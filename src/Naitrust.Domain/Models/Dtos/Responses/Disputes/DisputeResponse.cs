namespace Naitrust.Domain.Models.Dtos.Responses.Disputes;

/// <summary>
/// Frontend DealDispute: {dealId, status, reason, description, openedByName, createdAt, initialDecisionDueAt, messages[]}
/// </summary>
public record DisputeResponse(
    Guid DealId,
    string Status,
    string Reason,
    string Description,
    string OpenedByName,
    DateTime CreatedAt,
    List<DisputeMessageDto>? Messages,
    /// <summary>Deadline for Naitrust's first decision: CreatedAt + 2 business days.</summary>
    DateTime? InitialDecisionDueAt = null);

public record DisputeMessageDto(
    Guid Id,
    string ByName,
    bool ByYou,
    string Body,
    DateTime CreatedAt);
