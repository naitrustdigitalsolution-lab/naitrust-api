namespace Naitrust.Application.ExternalServices.OpenAi;

public class OpenAiProviderService : IAiProviderService
{
    public Task<string> GetCompletionAsync(string prompt, string systemMessage, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<T> GetStructuredOutputAsync<T>(string prompt, string systemMessage, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<float[]> GetEmbeddingAsync(string text, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<object> ModerateTextAsync(string text, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
