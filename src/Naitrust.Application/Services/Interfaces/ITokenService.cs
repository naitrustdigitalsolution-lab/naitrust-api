using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken ct = default);
    Task<bool> RevokeRefreshTokenAsync(string token, CancellationToken ct = default);
}
