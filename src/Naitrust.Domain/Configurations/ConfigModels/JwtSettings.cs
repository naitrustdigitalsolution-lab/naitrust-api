namespace Naitrust.Domain.Configurations.ConfigModels;

public class JwtSettings
{
    public string Secret { get; set; } = "";
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public int ExpiresInMinutes { get; set; }
    public int RefreshTokenExpiresInDays { get; set; }
}
