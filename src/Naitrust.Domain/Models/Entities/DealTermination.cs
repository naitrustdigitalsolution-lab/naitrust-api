namespace Naitrust.Domain.Models.Entities;

public class DealTermination : BaseEntity
{
    public Guid DealId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string Reason { get; set; } = default!;
    public string Status { get; set; } = "requested"; // requested, accepted, rejected
    public Guid? RespondedByUserId { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? ResponseReason { get; set; }
}
