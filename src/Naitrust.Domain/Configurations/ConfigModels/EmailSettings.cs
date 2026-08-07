namespace Naitrust.Domain.Configurations.ConfigModels;

public class EmailSettings
{
    public string Provider { get; set; } = "";
    public string? Host { get; set; }
    public int? Port { get; set; }
    public string? ApiKey { get; set; }
    public string FromAddress { get; set; } = "";
    public string FromName { get; set; } = "";
    public string? ReplyToAddress { get; set; }
}
