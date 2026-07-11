using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class Transaction : BaseEntity
{
    public string Reference { get; set; } = default!;
    public Guid TransactionTypeId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? BusinessId { get; set; }
    public PartyMode PartyMode { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public TransactionCategory Category { get; set; }
    public long AmountMinor { get; set; }
    public long FeeMinor { get; set; }
    public string Currency { get; set; } = default!;
    public TransactionStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public VerificationLevel? VerificationLevelRequired { get; set; }
    public RiskLevel? RiskLevel { get; set; }
    public Guid? AgreementId { get; set; }
    public DateTime? TermsAcceptedAt { get; set; }
    public DateTime? AutoConfirmAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}
