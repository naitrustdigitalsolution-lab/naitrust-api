using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Configurations.ConfigModels;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Auth;
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
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        UserManager<NaitrustUser> userManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
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
            PhoneNumber = request.Phone,
            Status = UserStatus.Active,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<AuthResponse>.BadRequest($"Registration failed: {errors}");
        }

        await _userManager.AddToRoleAsync(user, "User");
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

        return NaitrustResponse<AuthResponse>.Created(
            "Registration successful.",
            new AuthResponse(accessToken, refreshTokenValue, DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)));
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

        return NaitrustResponse<AuthResponse>.Success(
            "Login successful.",
            new AuthResponse(accessToken, refreshTokenValue, DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)));
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

    public async Task<NaitrustResponse<UserProfileResponse>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<UserProfileResponse>.NotFound("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return NaitrustResponse<UserProfileResponse>.Success("User profile retrieved.", new UserProfileResponse(
            user.Id,
            user.Email!,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            roles,
            user.Status.ToString(),
            user.EmailVerifiedAt,
            user.PhoneVerifiedAt,
            user.IdentityVerifiedAt,
            user.LastLivenessVerifiedAt));
    }

    public async Task<NaitrustResponse<bool>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default)
    {
        var parts = request.Token.Split(':', 2);
        if (parts.Length != 2)
        {
            return NaitrustResponse<bool>.BadRequest("Invalid verification token.");
        }

        var user = await _userManager.FindByIdAsync(parts[0]);
        if (user is null)
        {
            return NaitrustResponse<bool>.BadRequest("Invalid verification token.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, parts[1]);
        if (!result.Succeeded)
        {
            return NaitrustResponse<bool>.BadRequest("Email verification failed.");
        }

        user.EmailVerifiedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return NaitrustResponse<bool>.Success("Email verified successfully.", true);
    }

    public async Task<NaitrustResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            // Return success even if user doesn't exist to prevent email enumeration
            return NaitrustResponse<bool>.Success("If this email exists, a reset link has been sent.", true);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // TODO: Send email with reset link containing token
        Console.WriteLine($"[DEV] Password reset token for {user.Email}: {user.Id}:{token}");

        return NaitrustResponse<bool>.Success("If this email exists, a reset link has been sent.", true);
    }

    public async Task<NaitrustResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        var parts = request.Token.Split(':', 2);
        if (parts.Length != 2)
        {
            return NaitrustResponse<bool>.BadRequest("Invalid reset token.");
        }

        var user = await _userManager.FindByIdAsync(parts[0]);
        if (user is null)
        {
            return NaitrustResponse<bool>.BadRequest("Invalid reset token.");
        }

        var result = await _userManager.ResetPasswordAsync(user, parts[1], request.NewPassword);
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

        return NaitrustResponse<AuthResponse>.Success(
            "Token refreshed successfully.",
            new AuthResponse(accessToken, newRefreshTokenValue, DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)));
    }
}
