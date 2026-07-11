namespace Naitrust.Domain.Models.Dtos.Requests.Verification;

public record OwnershipVerificationRequest(Guid BusinessId, string Method);
