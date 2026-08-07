namespace Naitrust.Domain.Models.Dtos.Responses.Negotiations;

public record NegotiationResponse(
    Guid DealId,
    string Status,
    List<NegotiationProposalResponse> Proposals);
