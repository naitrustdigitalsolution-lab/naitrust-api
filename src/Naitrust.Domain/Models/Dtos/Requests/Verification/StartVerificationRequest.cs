namespace Naitrust.Domain.Models.Dtos.Requests.Verification;

public record StartVerificationRequest(
    string SubjectType,
    Guid SubjectId,
    string VerificationType,
    string? VerificationLevel,
    Guid? TransactionId);
