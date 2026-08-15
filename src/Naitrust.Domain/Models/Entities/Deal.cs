using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class Deal : BaseEntity
{
    public string Reference { get; set; } = default!;
    public Guid? TransactionTypeId { get; set; }
    public string? UseCase { get; set; }
    public DealType DealType { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? BusinessId { get; set; }
    public PartyMode PartyMode { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DealCategory Category { get; set; }
    public long AmountMinor { get; set; }
    public long FeeMinor { get; set; }
    public string Currency { get; set; } = default!;
    public DealStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public VerificationLevel? VerificationLevelRequired { get; set; }
    public RiskLevel? RiskLevel { get; set; }
    public Guid? AgreementId { get; set; }
    public string? DeliveryDueDate { get; set; }
    public string? ReleaseConditions { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? ExtendedProductTestingDays { get; set; }
    public bool Recurring { get; set; }
    public string? PreviousReference { get; set; }
    public DateTime? TermsAcceptedAt { get; set; }
    public DateTime? AutoConfirmAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    /// <summary>Amount due for the first funding stage. Null for a single, non-staged payment.</summary>
    public long? InitialPaymentMinor { get; set; }
    /// <summary>Balance tracked for the second funding stage.</summary>
    public long? RemainingPaymentMinor { get; set; }
    /// <summary>Condition that must be met before the remaining balance can be funded or released.</summary>
    public string? NextPaymentReleaseConditions { get; set; }
    /// <summary>Which payment stage is currently active (1 or 2). Null for a single-payment deal.</summary>
    public int? ActivePaymentStage { get; set; }
    public DateTime? FirstPaymentReleasedAt { get; set; }
}
