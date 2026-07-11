using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Auth;
using Naitrust.Domain.Models.Dtos.Responses.Auth;

namespace Naitrust.Application.Services.Implementations.Auth;

public class AuthService : IAuthService
{
    public Task<NaitrustResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> LogoutAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<UserProfileResponse>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
