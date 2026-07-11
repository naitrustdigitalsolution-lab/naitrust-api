using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Domain.Models.Entities;

public class ReleaseRequest : BaseEntity
{
    public Guid TransactionId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string? Provider { get; set; }
    public string? ProviderReference { get; set; }
    public ReleaseRequestStatus Status { get; set; }
    public string? Reason { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
