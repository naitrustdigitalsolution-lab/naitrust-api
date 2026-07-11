using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class VerificationDocument : BaseEntity
{
    public Guid VerificationRequestId { get; set; }
    public Guid UploadedByUserId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string FileUrl { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public long FileSize { get; set; }
    public VerificationDocumentStatus Status { get; set; }
    public string? ReviewNotes { get; set; }
}
