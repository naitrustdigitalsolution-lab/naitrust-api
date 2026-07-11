namespace Naitrust.Domain.Models.Enums.Payments;

public enum LedgerEventType
{
    FundingConfirmed,
    ReleaseApproved,
    FeeRecognized,
    SellerPayoutExecuted,
    BuyerRefundExecuted,
    SplitResolutionExecuted,
    FeeSwept,
    ReconciliationAdjustment
}
