namespace Naitrust.Domain.Models.Dtos.Requests.InstantTransfers;

public record CreateInstantTransferRequest(
    TransferRecipientInput Recipient,
    long AmountMinor,
    string Currency,
    string? Narration = null);

public record TransferRecipientInput(
    string Method,
    string Identifier,
    string? ResolvedName = null,
    string? BankName = null,
    string? NaitrustAccountNumber = null,
    string? NaitrustId = null,
    string? AccountType = null,
    bool IdentityVerified = false);
