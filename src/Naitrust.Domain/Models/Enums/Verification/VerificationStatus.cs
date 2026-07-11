namespace Naitrust.Domain.Models.Enums.Verification;

public enum VerificationStatus
{
    NotStarted,
    Pending,
    PaymentPending,
    Processing,
    Verified,
    NeedsMoreInfo,
    ManualReview,
    Rejected,
    Expired
}
