namespace Naitrust.Domain.Models.Dtos.Responses.Admin;

public record AdminVerificationResponse(
    Guid Id,
    string SubjectType,
    Guid SubjectId,
    string VerificationType,
    string Status,
    Guid? ReviewedBy,
    DateTime? ReviewedAt,
    DateTime CreatedAt);
