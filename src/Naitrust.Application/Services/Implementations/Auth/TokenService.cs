using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Implementations.Auth;

public class TokenService : ITokenService
{
    public string GenerateAccessToken(User user)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RevokeRefreshTokenAsync(string token, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
