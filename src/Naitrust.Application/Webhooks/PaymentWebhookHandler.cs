namespace Naitrust.Application.Webhooks;

public class PaymentWebhookHandler : IWebhookHandler
{
    public Task HandleAsync(string partner, string eventType, string payload, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
