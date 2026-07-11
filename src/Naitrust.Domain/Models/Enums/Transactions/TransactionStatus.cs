namespace Naitrust.Domain.Models.Enums.Transactions;

public enum TransactionStatus
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
