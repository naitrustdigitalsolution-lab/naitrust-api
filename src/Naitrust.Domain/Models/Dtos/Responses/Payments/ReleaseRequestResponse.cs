namespace Naitrust.Domain.Models.Dtos.Responses.Payments;

public record ReleaseRequestResponse(
    Guid Id,
    Guid TransactionId,
    Guid RequestedByUserId,
    string Status,
    decimal Amount,
    string? Reason,
    DateTime CreatedAt);
