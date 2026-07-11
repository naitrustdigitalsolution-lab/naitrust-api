namespace Naitrust.Application.ExternalServices;

public interface IAiProviderService
{
    Task<string> GetCompletionAsync(string prompt, string systemMessage, CancellationToken ct = default);
    Task<T> GetStructuredOutputAsync<T>(string prompt, string systemMessage, CancellationToken ct = default);
    Task<float[]> GetEmbeddingAsync(string text, CancellationToken ct = default);
    Task<object> ModerateTextAsync(string text, CancellationToken ct = default);
}
