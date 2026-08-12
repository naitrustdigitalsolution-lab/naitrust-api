namespace Naitrust.Domain.Models.Dtos.Responses.Wallet;

public record WalletActivityResponse(
    Guid Id,
    string Kind,
    long AmountMinor,
    string Currency,
    string Description,
    DateTime CreatedAt);
