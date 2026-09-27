namespace Naitrust.Domain.Configurations.ConfigModels;

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = [];
    public string[] AdditionalAllowedOrigins { get; set; } = [];
}
