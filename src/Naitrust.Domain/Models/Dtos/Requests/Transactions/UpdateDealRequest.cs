namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record UpdateDealRequest(string? Title, string? Description, long? AmountMinor);
