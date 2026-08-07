namespace Naitrust.Domain.Models.Dtos.Responses.Negotiations;

public record NegotiationProposalResponse(
    Guid Id,
    string ByName,
    bool ByYou,
    string Message,
    ProposedChangesResponse Changes,
    string Status,
    DateTime CreatedAt);

public record ProposedChangesResponse(
    long? AmountMinor,
    string? DeliveryDueDate,
    string? ReleaseConditions,
    string? AgreementNote);
