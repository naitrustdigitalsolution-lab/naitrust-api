namespace Naitrust.Domain.Configurations.ConfigModels;

public class StorageSettings
{
    public string Provider { get; set; } = "";
    public string? BaseUrl { get; set; }
    public string? PublicKey { get; set; }
    public string? PrivateKey { get; set; }
    public string? UrlEndpoint { get; set; }
}
