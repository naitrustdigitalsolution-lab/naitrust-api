using Microsoft.AspNetCore.Identity;

namespace Naitrust.Domain.Models.Entities;

public class NaitrustRoleClaim : IdentityRoleClaim<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
