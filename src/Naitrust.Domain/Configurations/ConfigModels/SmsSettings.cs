namespace Naitrust.Domain.Configurations.ConfigModels;

public class SmsSettings
{
    public string Provider { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public string? SenderId { get; set; }
}
