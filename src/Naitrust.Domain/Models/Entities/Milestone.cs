using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

public class Milestone : BaseEntity
{
    public Guid TransactionId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public long AmountMinor { get; set; }
    public DateTime? DueAt { get; set; }
    public MilestoneStatus Status { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
}
