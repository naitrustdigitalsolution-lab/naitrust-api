namespace Naitrust.Domain.Configurations.ConfigModels;

public class OpenAiSettings
{
    public string ApiKey { get; set; } = "";
    public string DefaultModel { get; set; } = "";
    public string? EmbeddingModel { get; set; }
}
