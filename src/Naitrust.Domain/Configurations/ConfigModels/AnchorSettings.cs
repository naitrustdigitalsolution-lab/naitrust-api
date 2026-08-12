namespace Naitrust.Domain.Configurations.ConfigModels;

public class AnchorSettings
{
    public string ApiKey { get; set; } = "";
    public string BaseUrl { get; set; } = "https://api.sandbox.getanchor.co/api/v1/";
    public string WebhookSecret { get; set; } = "";
    public bool Sandbox { get; set; } = true;
    /// <summary>
    /// The Anchor customer ID for the Naitrust platform itself.
    /// Used to own the platform escrow subledger. Configure once in settings.
    /// </summary>
    public string PlatformCustomerId { get; set; } = "";
}
