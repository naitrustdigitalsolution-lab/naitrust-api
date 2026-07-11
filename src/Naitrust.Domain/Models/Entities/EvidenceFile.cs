using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class EvidenceFile
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Guid? MilestoneId { get; set; }
    public Guid UploadedByUserId { get; set; }
    public EvidenceType Type { get; set; }
    public string FileUrl { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
