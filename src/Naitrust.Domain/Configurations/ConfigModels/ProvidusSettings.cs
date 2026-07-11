namespace Naitrust.Domain.Configurations.ConfigModels;

public class ProvidusSettings
{
    public string BaseUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string WebhookSecret { get; set; } = "";
    public bool Sandbox { get; set; }
}
