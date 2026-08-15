namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealDeliveryLifecycleDto(
    DealDeliveryCardDto? Card,
    DealHandoverReviewDto Handover,
    DealFundingReviewDto FundingReview);

public record DealDeliveryCardDto(
    string Token,
    string OtpCode,
    Guid? IntendedBuyerUserId,
    DateTime GeneratedAt,
    DateTime ExpiresAt,
    string Status,
    int Generation,
    DateTime? UsedAt,
    DateTime? InvalidatedAt);

public record DealHandoverReviewDto(
    string Status,
    DateTime? ReceivedAt,
    DateTime? EndsAt,
    DateTime? CompletedAt,
    string? CompletionReason);

public record DealFundingReviewDto(
    string Status,
    DateTime? StartsAt,
    DateTime? EndsAt,
    int? ExtendedProductTestingDays,
    DateTime? ReleaseApprovedAt,
    DateTime? PaidOutAt,
    string? ReleaseMethod,
    string? PaymentReference);
