using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class Business : BaseEntity
{
    public Guid OwnerUserId { get; set; }
    public string Name { get; set; } = default!;
    public string? Slug { get; set; }
    public string? NtId { get; set; }
    public string Type { get; set; } = default!;
    public string? Description { get; set; }
    public string? OwnerName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? TaxId { get; set; }
    public string Country { get; set; } = default!;
    public string? State { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? SocialHandles { get; set; }
    public string? PaymentAccountBankName { get; set; }
    public string? PaymentAccountNumber { get; set; }
    public string? PaymentAccountName { get; set; }
    public BusinessVerificationStatus VerificationStatus { get; set; }
    public DateTime? BusinessVerifiedAt { get; set; }
    public DateTime? OwnershipVerifiedAt { get; set; }
    public DateTime? VerificationExpiresAt { get; set; }
    public RiskLevel? RiskLevel { get; set; }
}
