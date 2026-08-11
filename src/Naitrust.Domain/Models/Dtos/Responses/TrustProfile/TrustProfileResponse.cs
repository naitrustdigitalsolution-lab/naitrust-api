namespace Naitrust.Domain.Models.Dtos.Responses.TrustProfile;

public record TrustProfileResponse(
    string IdentityVerificationLevel,
    string BusinessVerificationLevel,
    int CompletedDealsCount,
    int CancelledDealsCount,
    int ActiveDealsCount,
    int ResolvedDisputesCount,
    int RepeatCounterpartiesCount,
    string MemberSince,
    double? AverageCompletionDays,
    double? AverageResponseTimeHours,
    double? RatingAverage,
    int RatingCount);
