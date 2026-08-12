namespace Naitrust.Domain.Models.Dtos.Requests.Negotiations;

/// <summary>
/// Accept or decline a specific proposal.
/// POST /transactions/{txnId}/negotiation/proposals/{proposalId}
/// </summary>
public record RespondToProposalRequest(string Action); // "accepted" | "declined"
