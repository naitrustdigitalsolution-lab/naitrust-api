namespace Naitrust.Domain.Models.Dtos.Responses.Businesses;

public record BusinessResponse(
    Guid Id,
    Guid OwnerUserId,
    string Name,
    string? Slug,
    string? NtId,
    string Category,
    string? Description,
    string? OwnerName,
    string? Email,
    string? Phone,
    string? Website,
    string? RcNumber,
    string? TaxId,
    string Country,
    string? State,
    string? City,
    string? Address,
    List<SocialHandleDto>? SocialHandles,
    PaymentAccountDto? PaymentAccount,
    string VerificationStatus,
    bool Verified,
    string? RiskLevel,
    DateTime? BusinessVerifiedAt,
    DateTime? OwnershipVerifiedAt,
    DateTime? VerificationExpiresAt,
    DateTime CreatedAt);

public record SocialHandleDto(string Platform, string Value);

public record PaymentAccountDto(string BankName, string AccountNumber, string AccountName);
