using Naitrust.Domain.Models.Dtos.Requests.Auth;
using Naitrust.Domain.Models.Dtos.Responses.Auth;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAuthService
{
    Task<NaitrustResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> LogoutAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<UserProfileResponse>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
}
