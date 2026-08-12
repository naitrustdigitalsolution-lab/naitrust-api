namespace Naitrust.Domain.Models.Enums.Transactions;

public enum DealStatus
{
    Draft,
    PendingCounterparty,
    TermsNegotiation,
    TermsAgreed,
    AwaitingFunding,
    Funded,
    InProgress,
    EvidenceSubmitted,
    BuyerReview,
    ReleaseApproved,
    Disputed,
    PaidOut,
    Refunded,
    Cancelled,
    Completed
}
