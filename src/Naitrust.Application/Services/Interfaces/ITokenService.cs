using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Generates a signed JWT access token containing user claims (sub, email, name, roles).
    /// </summary>
    string GenerateAccessToken(NaitrustUser user, IList<string> roles);

    /// <summary>
    /// Generates a cryptographically secure random refresh token string.
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates that a refresh token exists, has not been revoked, and has not expired.
    /// </summary>
    Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken ct = default);

    /// <summary>
    /// Revokes a refresh token by setting its RevokedAt timestamp to the current UTC time.
    /// </summary>
    Task<bool> RevokeRefreshTokenAsync(string token, CancellationToken ct = default);
}
