using Microsoft.AspNetCore.Identity;

namespace Naitrust.Domain.Models.Entities;

public class NaitrustRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<NaitrustRoleClaim> RoleClaims { get; set; } = new List<NaitrustRoleClaim>();
}
