namespace Naitrust.Domain.Models.Dtos.Responses.PaymentRequests;

public record PaymentRequestResponse(
    Guid Id,
    string Reference,
    string RequestedFromName,
    long AmountMinor,
    string Currency,
    string? Reason,
    string Status,
    DateTime CreatedAt,
    DateTime ExpiresAt);
