namespace Naitrust.Domain.Models.Dtos.Requests.Wallet;

public record FundWalletRequest(Guid LinkedBankAccountId, long AmountMinor);
