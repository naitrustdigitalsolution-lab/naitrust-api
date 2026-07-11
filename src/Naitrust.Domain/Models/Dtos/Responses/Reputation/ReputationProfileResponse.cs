namespace Naitrust.Domain.Models.Dtos.Responses.Reputation;

public record ReputationProfileResponse(
    Guid Id,
    string SubjectType,
    Guid SubjectId,
    int CompletedTransactionsCount,
    int DisputedTransactionsCount,
    int CancelledTransactionsCount,
    long TotalCompletedValue,
    decimal RatingAverage,
    int RatingCount);
