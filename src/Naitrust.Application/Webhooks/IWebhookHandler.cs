namespace Naitrust.Application.Webhooks;

public interface IWebhookHandler
{
    Task HandleAsync(string partner, string eventType, string payload, CancellationToken ct = default);
}
