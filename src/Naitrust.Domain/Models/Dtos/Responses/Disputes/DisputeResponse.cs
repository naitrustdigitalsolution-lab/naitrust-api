namespace Naitrust.Domain.Models.Dtos.Responses.Disputes;

public record DisputeResponse(
    Guid Id,
    Guid TransactionId,
    Guid OpenedByUserId,
    string Status,
    string Reason,
    string? Description,
    string? Resolution,
    DateTime? ResolvedAt,
    List<DisputeMessageResponse>? Messages,
    DateTime CreatedAt);
