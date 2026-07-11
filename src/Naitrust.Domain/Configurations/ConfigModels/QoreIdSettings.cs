namespace Naitrust.Domain.Configurations.ConfigModels;

public class QoreIdSettings
{
    public string BaseUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string SecretKey { get; set; } = "";
    public bool Sandbox { get; set; }
}
