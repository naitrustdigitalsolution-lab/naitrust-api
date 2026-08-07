using System.Text.Json;

namespace Naitrust.Domain.Models.Dtos.Requests.Security;

public record SubmitKycRequest(
    string Kind,                       // "individual" | "business"
    JsonElement? Payload = null        // Arbitrary KYC payload — passed through to verification
);
