namespace Naitrust.Domain.Models.Dtos.Responses.Wallet;

public record LinkedBankAccountResponse(
    Guid Id,
    string BankName,
    string AccountNumber,
    string AccountName,
    bool IsDefault);
