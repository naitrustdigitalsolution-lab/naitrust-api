namespace Naitrust.Domain.Models.Dtos.Responses.Verification;

public record VerificationRequestResponse(
    Guid Id,
    string SubjectType,
    Guid SubjectId,
    string VerificationType,
    string VerificationLevel,
    string Status,
    List<VerificationStepResponse>? Steps,
    DateTime CreatedAt);
