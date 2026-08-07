namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealTerminationResponse(
    Guid DealId,
    string Status,
    string Reason,
    string RequestedByName,
    bool RequestedByYou,
    DateTime CreatedAt,
    string? RespondedByName,
    DateTime? RespondedAt,
    string? ResponseReason);
