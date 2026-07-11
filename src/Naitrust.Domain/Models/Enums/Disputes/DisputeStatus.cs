namespace Naitrust.Domain.Models.Enums.Disputes;

public enum DisputeStatus
{
    None,
    Opened,
    EvidenceRequested,
    UnderReview,
    ResolvedRelease,
    ResolvedRefund,
    ResolvedSplit,
    Closed
}
