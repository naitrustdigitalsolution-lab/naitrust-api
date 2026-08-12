namespace Naitrust.Domain.Models.Dtos.Requests.Wallet;

public record WithdrawRequest(
    Guid LinkedBankAccountId,
    long AmountMinor);
