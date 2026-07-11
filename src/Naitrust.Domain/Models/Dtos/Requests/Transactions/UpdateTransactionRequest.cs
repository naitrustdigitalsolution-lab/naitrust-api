namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record UpdateTransactionRequest(string? Title, string? Description, long? AmountMinor);
