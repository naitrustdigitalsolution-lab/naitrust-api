using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Naitrust.Application.ExternalServices;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Configurations.ConfigModels;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Auth;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;
using Naitrust.Domain.Models.Dtos.Responses.Auth;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<NaitrustUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;
    private readonly IBusinessService _businessService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        UserManager<NaitrustUser> userManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ICacheService cacheService,
        IEmailService emailService,
        IBusinessService businessService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _emailService = emailService;
        _businessService = businessService;
        _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt settings are not configured");
    }

    public async Task<NaitrustResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return NaitrustResponse<AuthResponse>.Conflict("A user with this email already exists.");
        }

        var user = new NaitrustUser
        {
            Email = request.Email.ToLowerInvariant(),
            UserName = request.Email.ToLowerInvariant(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Status = UserStatus.Active,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<AuthResponse>.BadRequest($"Registration failed: {errors}");
        }

        var roleName = !string.IsNullOrWhiteSpace(request.Role) ? request.Role : "User";
        await _userManager.AddToRoleAsync(user, roleName);
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
            CreatedAt = DateTime.UtcNow
        };

        var repo = _unitOfWork.GetRepository<RefreshToken>();
        await repo.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        // Auto-create business if pending business data provided
        if (request.PendingBusinessData is not null)
        {
            var bizData = request.PendingBusinessData;
            var createBizRequest = new CreateBusinessRequest(
                Name: bizData.Name,
                Type: bizData.Category ?? "General",
                Slug: null,
                Description: null,
                OwnerName: $"{request.FirstName} {request.LastName}".Trim(),
                Email: request.Email,
                Phone: request.PhoneNumber,
                PhoneNumber: request.PhoneNumber,
                Website: null,
                RegistrationNumber: bizData.RegistrationNumber,
                TaxId: null,
                Country: bizData.Country ?? "NG",
                State: bizData.State,
                City: bizData.City,
                Address: bizData.Address,
                SocialHandles: null,
                PaymentAccountBankName: null,
                PaymentAccountNumber: null,
                PaymentAccountName: null);

            await _businessService.CreateBusinessAsync(user.Id, createBizRequest, ct);
        }

        // Generate and send email verification OTP
        var otp = await GenerateOtpAsync(user.Email!, "email-verify", ct);
        await _emailService.SendVerificationOtpAsync(user.Email!, user.FirstName, user.Email!, otp, ct);

        var userResponse = BuildUserResponse(user, roles);
        return NaitrustResponse<AuthResponse>.Created(
            "Registration successful.",
            new AuthResponse(userResponse, accessToken, refreshTokenValue));
    }

    public async Task<NaitrustResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<AuthResponse>.Unauthorized("Invalid email or password.");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            return NaitrustResponse<AuthResponse>.Unauthorized("Invalid email or password.");
        }

        // Block login if email is not verified — resend OTP
        if (!user.EmailConfirmed)
        {
            var otp = await GenerateOtpAsync(user.Email!, "email-verify", ct);
            await _emailService.SendVerificationOtpAsync(user.Email!, user.FirstName, user.Email!, otp, ct);
            return NaitrustResponse<AuthResponse>.Forbidden("Please verify your email. A new verification code has been sent.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
            CreatedAt = DateTime.UtcNow
        };

        var repo = _unitOfWork.GetRepository<RefreshToken>();
        await repo.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        var userResponse = BuildUserResponse(user, roles);
        return NaitrustResponse<AuthResponse>.Success(
            "Login successful.",
            new AuthResponse(userResponse, accessToken, refreshTokenValue));
    }

    public async Task<NaitrustResponse<bool>> LogoutAsync(Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<RefreshToken>();
        var activeTokens = await repo.GetAllDataAsync(
            x => x.UserId == userId && x.RevokedAt == null);

        foreach (var token in activeTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
            await repo.UpdateAsync(token);
        }

        await _unitOfWork.SaveChangesAsync();
        return NaitrustResponse<bool>.Success("Logged out successfully.", true);
    }

    public async Task<NaitrustResponse<ProfileResponse>> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<ProfileResponse>.NotFound("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        return NaitrustResponse<ProfileResponse>.Success("User profile retrieved.", new ProfileResponse(BuildUserResponse(user, roles)));
    }

    public async Task<NaitrustResponse<FrontendUserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<FrontendUserResponse>.NotFound("User not found.");
        }

        if (request.FirstName is not null)
        {
            user.FirstName = request.FirstName;
        }

        if (request.LastName is not null)
        {
            user.LastName = request.LastName;
        }

        if (request.PhoneNumber is not null)
        {
            user.PhoneNumber = request.PhoneNumber;
        }
        if (request.Bio is not null)
        {
            user.Bio = request.Bio;
        }
        if (request.Address is not null)
        {
            user.Address = request.Address;
        }
        if (request.City is not null)
        {
            user.City = request.City;
        }
        if (request.State is not null)
        {
            user.State = request.State;
        }
        if (request.Country is not null)
        {
            user.Country = request.Country;
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        return NaitrustResponse<FrontendUserResponse>.Success("Profile updated successfully.", BuildUserResponse(user, roles));
    }

    public async Task<NaitrustResponse<AuthResponse>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NaitrustResponse<AuthResponse>.BadRequest("Invalid email or OTP.");
        }

        var isValid = await ValidateOtpAsync(user.Email!, request.Otp, "email-verify", ct);
        if (!isValid)
        {
            return NaitrustResponse<AuthResponse>.BadRequest("Invalid or expired OTP.");
        }

        user.EmailVerifiedAt = DateTime.UtcNow;
        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
            CreatedAt = DateTime.UtcNow
        };

        var repo = _unitOfWork.GetRepository<RefreshToken>();
        await repo.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        // Send welcome email after successful verification
        var role = roles.FirstOrDefault() ?? "User";
        await _emailService.SendWelcomeEmailAsync(user.Email!, user.FirstName, role, ct);

        var userResponse = BuildUserResponse(user, roles);
        return NaitrustResponse<AuthResponse>.Success("Email verified successfully.", new AuthResponse(userResponse, accessToken, refreshTokenValue));
    }

    public async Task<NaitrustResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            // Return success even if user doesn't exist to prevent email enumeration
            return NaitrustResponse<bool>.Success("If this email exists, a reset OTP has been sent.", true);
        }

        var otp = await GenerateOtpAsync(user.Email!, "password-reset", ct);
        var userName = $"{user.FirstName} {user.LastName}".Trim();
        await _emailService.SendPasswordResetOtpAsync(user.Email!, userName, otp, ct);

        return NaitrustResponse<bool>.Success("If this email exists, a reset OTP has been sent.", true);
    }

    public async Task<NaitrustResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NaitrustResponse<bool>.BadRequest("Invalid reset token.");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<bool>.BadRequest($"Password reset failed: {errors}");
        }

        return NaitrustResponse<bool>.Success("Password has been reset successfully.", true);
    }

    public async Task<NaitrustResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<RefreshToken>();
        var existingToken = await repo.GetSingleByAsync(
            x => x.Token == request.RefreshToken && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow);

        if (existingToken is null)
        {
            return NaitrustResponse<AuthResponse>.Unauthorized("Invalid or expired refresh token.");
        }

        // Revoke the old token (rotation)
        existingToken.RevokedAt = DateTime.UtcNow;
        await repo.UpdateAsync(existingToken);

        var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<AuthResponse>.Unauthorized("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var newRefreshTokenValue = _tokenService.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        var userResponse = BuildUserResponse(user, roles);
        return NaitrustResponse<AuthResponse>.Success(
            "Token refreshed successfully.",
            new AuthResponse(userResponse, accessToken, newRefreshTokenValue));
    }

    public async Task<NaitrustResponse<AuthResponse>> Verify2FAAsync(Verify2FARequest request, CancellationToken ct = default)
    {
        NaitrustUser? user = null;
        if (request.UserId.HasValue)
        {
            user = await _userManager.FindByIdAsync(request.UserId.Value.ToString());
        }
        else if (!string.IsNullOrEmpty(request.Email))
        {
            user = await _userManager.FindByEmailAsync(request.Email);
        }

        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<AuthResponse>.Unauthorized("Invalid 2FA request.");
        }

        var isValid = await ValidateOtpAsync(user.Email!, request.Token, "2fa", ct);
        if (!isValid)
        {
            return NaitrustResponse<AuthResponse>.Unauthorized("Invalid or expired 2FA token.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
            CreatedAt = DateTime.UtcNow
        };

        var repo = _unitOfWork.GetRepository<RefreshToken>();
        await repo.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        var userResponse = BuildUserResponse(user, roles);
        return NaitrustResponse<AuthResponse>.Success("2FA verified successfully.", new AuthResponse(userResponse, accessToken, refreshTokenValue));
    }

    public async Task<NaitrustResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<bool>.NotFound("User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<bool>.BadRequest($"Password change failed: {errors}");
        }

        return NaitrustResponse<bool>.Success("Password changed successfully.", true);
    }

    public async Task<NaitrustResponse<VerifyOtpResponse>> VerifyOtpAsync(VerifyOtpRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NaitrustResponse<VerifyOtpResponse>.BadRequest("Invalid email or OTP.");
        }

        var isValid = await ValidateOtpAsync(user.Email!, request.Otp, "password-reset", ct);
        if (!isValid)
        {
            return NaitrustResponse<VerifyOtpResponse>.BadRequest("Invalid or expired OTP.");
        }

        // Generate a password reset token for the actual reset step
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        return NaitrustResponse<VerifyOtpResponse>.Success("OTP verified successfully.", new VerifyOtpResponse(resetToken));
    }

    public async Task<NaitrustResponse<bool>> ResendVerificationOtpAsync(ResendVerificationOtpRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            // Return success to prevent email enumeration
            return NaitrustResponse<bool>.Success("If this email exists, a verification OTP has been sent.", true);
        }

        if (user.EmailConfirmed)
        {
            return NaitrustResponse<bool>.BadRequest("Email is already verified.");
        }

        var otp = await GenerateOtpAsync(user.Email!, "email-verify", ct);
        await _emailService.SendVerificationOtpAsync(user.Email!, user.FirstName, user.Email!, otp, ct);

        return NaitrustResponse<bool>.Success("If this email exists, a verification OTP has been sent.", true);
    }

    // --- Helpers ---

    private static int ComputeKycLevel(NaitrustUser user)
    {
        if (user.KycLevel.HasValue) { return user.KycLevel.Value; }
        var level = 0;
        if (user.EmailVerifiedAt.HasValue) { level = 1; }
        if (user.PhoneVerifiedAt.HasValue) { level = 2; }
        if (user.IdentityVerifiedAt.HasValue) { level = 3; }
        return level;
    }

    private FrontendUserResponse BuildUserResponse(NaitrustUser user, IList<string> roles)
    {
        var role = roles.FirstOrDefault() ?? "User";
        var name = $"{user.FirstName} {user.LastName}".Trim();
        var kycLevel = ComputeKycLevel(user);

        return new FrontendUserResponse(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Name: name,
            Role: role,
            Phone: user.PhoneNumber,
            Avatar: user.Avatar,
            KycLevel: kycLevel,
            KycVerified: kycLevel >= 3,
            IsEmailVerified: user.EmailVerifiedAt.HasValue,
            IsPhoneVerified: user.PhoneVerifiedAt.HasValue);
    }

    private async Task<string> GenerateOtpAsync(string email, string purpose, CancellationToken ct)
    {
        var otp = Random.Shared.Next(100000, 999999).ToString();
        var cacheKey = $"otp:{purpose}:{email.ToLowerInvariant()}";
        await _cacheService.SetAsync(cacheKey, otp, TimeSpan.FromMinutes(10), ct);
        return otp;
    }

    private async Task<bool> ValidateOtpAsync(string email, string otp, string purpose, CancellationToken ct)
    {
        var cacheKey = $"otp:{purpose}:{email.ToLowerInvariant()}";
        var storedOtp = await _cacheService.GetAsync<string>(cacheKey, ct);

        if (storedOtp is null || storedOtp != otp)
        {
            return false;
        }

        await _cacheService.RemoveAsync(cacheKey, ct);
        return true;
    }
}
