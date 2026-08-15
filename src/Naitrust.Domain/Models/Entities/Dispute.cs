using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Disputes;

namespace Naitrust.Domain.Models.Entities;

public class Dispute : BaseEntity
{
    public Guid DealId { get; set; }
    public Guid OpenedByUserId { get; set; }
    public DisputeStatus Status { get; set; }
    public string Reason { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? AdminOwnerId { get; set; }
    public DisputeResolution? Resolution { get; set; }
    public DateTime? ResolvedAt { get; set; }
    /// <summary>Whether the reporter attached evidence when opening the dispute. Drives the initial status.</summary>
    public bool HasEvidence { get; set; }
}
