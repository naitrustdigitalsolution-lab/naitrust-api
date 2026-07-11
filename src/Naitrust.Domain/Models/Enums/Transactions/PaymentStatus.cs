namespace Naitrust.Domain.Models.Enums.Transactions;

public enum PaymentStatus
{
    NotStarted,
    VirtualAccountIssued,
    AwaitingFunding,
    PaymentConfirmedByPartner,
    ReleaseRequested,
    Released,
    Refunded,
    Failed
}
