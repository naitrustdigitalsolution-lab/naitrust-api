using Naitrust.Domain.Models.Dtos.Requests.Auth;
using Naitrust.Domain.Models.Dtos.Responses.Auth;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user account with the provided credentials and profile information.
    /// </summary>
    Task<NaitrustResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

    /// <summary>
    /// Authenticates a user with email and password, returning access and refresh tokens.
    /// </summary>
    Task<NaitrustResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);

    /// <summary>
    /// Logs out the current user by revoking all active refresh tokens.
    /// </summary>
    Task<NaitrustResponse<bool>> LogoutAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the profile of the currently authenticated user.
    /// </summary>
    Task<NaitrustResponse<UserProfileResponse>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Confirms a user's email address using a verification token.
    /// </summary>
    Task<NaitrustResponse<bool>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default);

    /// <summary>
    /// Initiates a password reset flow by generating a reset token for the given email.
    /// </summary>
    Task<NaitrustResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);

    /// <summary>
    /// Resets a user's password using a valid password reset token.
    /// </summary>
    Task<NaitrustResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);

    /// <summary>
    /// Exchanges a valid refresh token for a new access/refresh token pair (token rotation).
    /// </summary>
    Task<NaitrustResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
}
