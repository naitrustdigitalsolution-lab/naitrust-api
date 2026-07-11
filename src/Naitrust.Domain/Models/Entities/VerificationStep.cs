using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class VerificationStep : BaseEntity
{
    public Guid VerificationRequestId { get; set; }
    public VerificationStepType Step { get; set; }
    public string? Provider { get; set; }
    public VerificationStepStatus Status { get; set; }
    public string? Message { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
