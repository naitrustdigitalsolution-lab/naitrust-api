namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record CreateTransactionRequest(
    Guid TransactionTypeId,
    string PartyMode,
    string Title,
    string? Description,
    string Category,
    long AmountMinor,
    string Currency);
