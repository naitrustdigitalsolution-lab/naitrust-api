namespace Naitrust.Domain.Models.Entities;

public class DisputeEvidence
{
    public Guid Id { get; set; }
    public Guid DisputeId { get; set; }
    public Guid EvidenceFileId { get; set; }
    public Guid SubmittedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
