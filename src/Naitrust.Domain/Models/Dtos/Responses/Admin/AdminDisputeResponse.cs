namespace Naitrust.Domain.Models.Dtos.Responses.Admin;

public record AdminDisputeResponse(
    Guid Id,
    Guid TransactionId,
    string Status,
    string Reason,
    Guid? AdminOwnerId,
    string? Resolution,
    DateTime? ResolvedAt,
    DateTime CreatedAt);
