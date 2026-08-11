namespace Naitrust.Domain.Models.Dtos.Responses.InstantTransfers;

public record InstantTransferResponse(
    Guid Id,
    string Reference,
    RecipientDto Recipient,
    long AmountMinor,
    string Currency,
    long FeeMinor,
    string? Narration,
    string Status,
    string Provider,
    bool IsMock,
    DateTime CreatedAt,
    DateTime? CompletedAt);

public record RecipientDto(
    string Method,
    string Identifier,
    string? ResolvedName,
    string? BankName,
    string? NaitrustAccountNumber,
    string? NaitrustId,
    string? AccountType,
    bool IdentityVerified);

public record ValidateRecipientResponse(
    string Method,
    string Identifier,
    string ResolvedName,
    string? BankName,
    string? NaitrustAccountNumber,
    string? NaitrustId,
    string? AccountType,
    bool IdentityVerified);
