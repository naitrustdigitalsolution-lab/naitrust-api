namespace Naitrust.Domain.Models.Dtos.Requests.Payments;

public record RequestReleaseRequest(Guid TransactionId, string? Reason);
