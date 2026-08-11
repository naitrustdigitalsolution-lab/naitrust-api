using Naitrust.Domain.Models.Dtos.Requests.Auth;
using Naitrust.Domain.Models.Dtos.Responses.Auth;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAuthService
{
    Task<NaitrustResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> LogoutAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<ProfileResponse>> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<FrontendUserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<AuthResponse>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<AuthResponse>> Verify2FAAsync(Verify2FARequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<VerifyOtpResponse>> VerifyOtpAsync(VerifyOtpRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ResendVerificationOtpAsync(ResendVerificationOtpRequest request, CancellationToken ct = default);
}
