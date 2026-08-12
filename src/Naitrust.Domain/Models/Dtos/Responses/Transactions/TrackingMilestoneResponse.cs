namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

/// <summary>
/// Frontend DealMilestone: {id, title, description?, status, updatedByName?, at?}
/// status: 'done' | 'current' | 'pending'
/// </summary>
public record TrackingMilestoneResponse(
    Guid Id,
    string Title,
    string? Description,
    string Status,
    string? UpdatedByName,
    DateTime? At);
