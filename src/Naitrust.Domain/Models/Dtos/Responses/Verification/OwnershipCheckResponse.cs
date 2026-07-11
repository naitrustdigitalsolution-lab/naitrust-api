namespace Naitrust.Domain.Models.Dtos.Responses.Verification;

public record OwnershipCheckResponse(string Method, string Status, string? EvidenceSummary);
