namespace Naitrust.Domain.Models.Dtos.Responses.Disputes;

public record DisputeEvidenceResponse(
    Guid Id,
    Guid DisputeId,
    Guid SubmittedByUserId,
    string? Description,
    string? FileUrl,
    DateTime CreatedAt);
