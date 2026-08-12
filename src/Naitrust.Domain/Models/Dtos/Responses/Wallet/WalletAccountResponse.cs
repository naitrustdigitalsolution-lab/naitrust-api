namespace Naitrust.Domain.Models.Dtos.Responses.Wallet;

public record WalletAccountResponse(
    Guid Id,
    Guid OwnerUserId,
    Guid? BusinessId,
    WalletBalanceDto Balance,
    long TotalInflowMinor,
    long TotalOutflowMinor,
    long AccountLimitMinor,
    VirtualAccountInfoDto? VirtualAccount,
    DateTime CreatedAt);

public record WalletBalanceDto(
    long AvailableMinor,
    long PendingMinor,
    long ProtectedMinor,
    string Currency);

public record VirtualAccountInfoDto(
    string BankName,
    string AccountNumber,
    string AccountName);
