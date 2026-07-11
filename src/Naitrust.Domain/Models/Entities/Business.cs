using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class Business : BaseEntity
{
    public Guid OwnerUserId { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string? RegistrationNumber { get; set; }
    public string? TaxId { get; set; }
    public string Country { get; set; } = default!;
    public string? State { get; set; }
    public string? Address { get; set; }
    public BusinessVerificationStatus VerificationStatus { get; set; }
    public DateTime? BusinessVerifiedAt { get; set; }
    public DateTime? OwnershipVerifiedAt { get; set; }
    public DateTime? VerificationExpiresAt { get; set; }
    public RiskLevel? RiskLevel { get; set; }
}
