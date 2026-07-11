namespace Naitrust.Infrastructure.Security;

public interface IWebhookSignatureValidator
{
    bool ValidateSignature(string payload, string signature, string secret);
}
