namespace Naitrust.Domain.Models.Dtos.Requests.Negotiations;

/// <summary>
/// Used for both opening a negotiation and proposing counter-changes.
/// POST /transactions/{txnId}/negotiation/propose
/// </summary>
public record ProposeNegotiationRequest(string Message, ProposedChangesInput Changes);
