namespace Naitrust.Application.Webhooks;

public class VerificationWebhookHandler : IWebhookHandler
{
    public Task HandleAsync(string partner, string eventType, string payload, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
