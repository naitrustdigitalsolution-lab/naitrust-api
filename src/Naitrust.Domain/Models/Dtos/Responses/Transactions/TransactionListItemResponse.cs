namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record TransactionListItemResponse(
    Guid Id,
    string Reference,
    string Title,
    long AmountMinor,
    string Currency,
    string Status,
    string PaymentStatus,
    string PartyMode,
    DateTime CreatedAt);
