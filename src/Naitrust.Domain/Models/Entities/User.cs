using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string PasswordHash { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public DateTime? PhoneVerifiedAt { get; set; }
    public DateTime? IdentityVerifiedAt { get; set; }
    public DateTime? LastLivenessVerifiedAt { get; set; }
    public DateTime? LastTransactionActivityAt { get; set; }
}
