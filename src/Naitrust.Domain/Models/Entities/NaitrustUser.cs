using Microsoft.AspNetCore.Identity;
using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class NaitrustUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public UserStatus Status { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public DateTime? PhoneVerifiedAt { get; set; }
    public DateTime? IdentityVerifiedAt { get; set; }
    public DateTime? LastLivenessVerifiedAt { get; set; }
    public DateTime? LastTransactionActivityAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Avatar { get; set; }
    public int? KycLevel { get; set; }
    public string? PinHash { get; set; }
    public string? TotpSecret { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
