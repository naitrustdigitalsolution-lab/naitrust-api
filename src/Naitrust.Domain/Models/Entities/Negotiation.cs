using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

public class Negotiation : BaseEntity
{
    public Guid DealId { get; set; }
    public Guid InitiatedByUserId { get; set; }
    public NegotiationStatus Status { get; set; }
    public Guid? LatestProposalId { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
