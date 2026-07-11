namespace Naitrust.Domain.Models.Dtos.Responses.Payments;

public record LedgerEntryResponse(
    Guid Id,
    Guid EntryGroupId,
    string EventType,
    string Account,
    long DebitMinor,
    long CreditMinor,
    string Currency,
    string? Memo,
    DateTime CreatedAt);
