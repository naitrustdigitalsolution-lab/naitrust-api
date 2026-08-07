using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

public class NegotiationProposal : BaseEntity
{
    public Guid NegotiationId { get; set; }
    public Guid ProposedByUserId { get; set; }
    public string? ProposedChangesJson { get; set; }
    public string? Message { get; set; }
    public ProposalStatus Status { get; set; }
    public DateTime? RespondedAt { get; set; }
}
