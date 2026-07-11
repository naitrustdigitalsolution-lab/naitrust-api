using System.Security.Cryptography;
using System.Text;

namespace Naitrust.Infrastructure.Security;

public class WebhookSignatureValidator : IWebhookSignatureValidator
{
    public bool ValidateSignature(string payload, string signature, string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(secretBytes);
        var computedHash = hmac.ComputeHash(payloadBytes);
        var computedSignature = Convert.ToHexString(computedHash).ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedSignature),
            Encoding.UTF8.GetBytes(signature.ToLowerInvariant()));
    }
}
