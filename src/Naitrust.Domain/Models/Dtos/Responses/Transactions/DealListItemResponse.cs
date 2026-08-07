namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealListItemResponse(
    Guid Id,
    string Reference,
    string Title,
    long AmountMinor,
    string Currency,
    string Status,
    string PaymentStatus,
    string PartyMode,
    string DealType,
    string? CounterpartyName,
    DateTime CreatedAt);
