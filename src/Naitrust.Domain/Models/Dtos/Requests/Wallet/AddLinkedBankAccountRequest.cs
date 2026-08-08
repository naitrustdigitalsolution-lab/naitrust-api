namespace Naitrust.Domain.Models.Dtos.Requests.Wallet;

public record AddLinkedBankAccountRequest(
    string BankCode,
    string AccountNumber,
    bool SetAsDefault = false);
