using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string? Metadata { get; set; }
    public DateTime? ReadAt { get; set; }
}
