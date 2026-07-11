using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class ReputationProfile : BaseEntity
{
    public SubjectType SubjectType { get; set; }
    public Guid SubjectId { get; set; }
    public int CompletedTransactionsCount { get; set; }
    public int DisputedTransactionsCount { get; set; }
    public int CancelledTransactionsCount { get; set; }
    public long TotalCompletedValue { get; set; }
    public decimal RatingAverage { get; set; }
    public int RatingCount { get; set; }
}
