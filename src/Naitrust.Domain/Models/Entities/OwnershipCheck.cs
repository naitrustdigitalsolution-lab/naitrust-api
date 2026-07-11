using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class OwnershipCheck : BaseEntity
{
    public Guid VerificationRequestId { get; set; }
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public OwnershipCheckMethod Method { get; set; }
    public OwnershipCheckStatus Status { get; set; }
    public string? EvidenceSummary { get; set; }
}
