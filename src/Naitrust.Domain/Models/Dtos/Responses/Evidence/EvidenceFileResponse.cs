namespace Naitrust.Domain.Models.Dtos.Responses.Evidence;

public record EvidenceFileResponse(
    Guid Id,
    Guid TransactionId,
    Guid? MilestoneId,
    Guid UploadedByUserId,
    string Type,
    string FileUrl,
    string FileName,
    string? Description,
    DateTime CreatedAt);
