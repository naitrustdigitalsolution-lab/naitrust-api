using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class VerificationRequest : BaseEntity
{
    public SubjectType SubjectType { get; set; }
    public Guid SubjectId { get; set; }
    public Guid? TransactionId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string? Provider { get; set; }
    public VerificationType VerificationType { get; set; }
    public VerificationLevel VerificationLevel { get; set; }
    public VerificationStatus Status { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentReference { get; set; }
    public string? ProviderReference { get; set; }
    public string? ResultSummary { get; set; }
    public string? RiskFlags { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public Guid? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
}
