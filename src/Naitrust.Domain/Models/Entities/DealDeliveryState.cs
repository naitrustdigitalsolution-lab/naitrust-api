using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

/// <summary>
/// Flattened 1:1 delivery-card / handover-review / funding-review state for a deal.
/// Kept as a single row (not three tables) since all three sections are always
/// read and written together — mirrors the frontend's DealDeliveryLifecycle shape,
/// which the response DTO re-nests on the way out.
/// </summary>
public class DealDeliveryState : BaseEntity
{
    public Guid DealId { get; set; }

    // Delivery card
    public string? CardToken { get; set; }
    public string? CardOtpCode { get; set; }
    public Guid? CardIntendedBuyerUserId { get; set; }
    public DateTime? CardGeneratedAt { get; set; }
    public DateTime? CardExpiresAt { get; set; }
    public DeliveryCardStatus? CardStatus { get; set; }
    public int CardGeneration { get; set; }
    public DateTime? CardUsedAt { get; set; }
    public DateTime? CardInvalidatedAt { get; set; }

    // Handover review
    public HandoverReviewStatus HandoverStatus { get; set; } = HandoverReviewStatus.NotStarted;
    public DateTime? HandoverReceivedAt { get; set; }
    public DateTime? HandoverEndsAt { get; set; }
    public DateTime? HandoverCompletedAt { get; set; }
    public HandoverCompletionReason? HandoverCompletionReason { get; set; }

    // Funding review
    public FundingReviewStatus FundingReviewStatus { get; set; } = FundingReviewStatus.NotStarted;
    public DateTime? FundingReviewStartsAt { get; set; }
    public DateTime? FundingReviewEndsAt { get; set; }
    public DateTime? ReleaseApprovedAt { get; set; }
    public DateTime? PaidOutAt { get; set; }
    public ReleaseMethod? ReleaseMethod { get; set; }
    public string? PaymentReference { get; set; }
}
