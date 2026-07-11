using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class BusinessMember : BaseEntity
{
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public BusinessMemberRole Role { get; set; }
    public BusinessMemberStatus Status { get; set; }
}
