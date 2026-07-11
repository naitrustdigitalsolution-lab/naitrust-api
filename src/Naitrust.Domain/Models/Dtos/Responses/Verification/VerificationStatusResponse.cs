namespace Naitrust.Domain.Models.Dtos.Responses.Verification;

public record VerificationStatusResponse(
    string SubjectType,
    Guid SubjectId,
    string OverallStatus,
    DateTime? IdentityVerifiedAt,
    DateTime? BusinessVerifiedAt,
    DateTime? OwnershipVerifiedAt,
    DateTime? LastLivenessVerifiedAt);
