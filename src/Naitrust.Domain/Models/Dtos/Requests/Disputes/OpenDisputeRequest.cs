namespace Naitrust.Domain.Models.Dtos.Requests.Disputes;

public record OpenDisputeRequest(Guid TransactionId, string Reason, string? Description);
