using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Configurations.ConfigModels;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Auth;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt settings are not configured");
        _unitOfWork = unitOfWork;
    }

    public string GenerateAccessToken(NaitrustUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<RefreshToken>();
        var refreshToken = await repo.GetSingleByAsync(
            x => x.Token == token && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow);
        return refreshToken is not null;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<RefreshToken>();
        var refreshToken = await repo.GetSingleByAsync(x => x.Token == token && x.RevokedAt == null);
        if (refreshToken is null)
        {
            return false;
        }

        refreshToken.RevokedAt = DateTime.UtcNow;
        await repo.UpdateAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
