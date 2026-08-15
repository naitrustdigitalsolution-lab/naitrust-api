using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

public class DealParty : BaseEntity
{
    public Guid DealId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? BusinessId { get; set; }
    public PartyType PartyType { get; set; }
    public PartyMode PartyMode { get; set; }
    public string DisplayName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public long? AllocationMinor { get; set; }
    /// <summary>This party's share of the first payment stage, for a staged/split-payment deal.</summary>
    public long? AllocationStage1Minor { get; set; }
    /// <summary>This party's share of the second payment stage, for a staged/split-payment deal.</summary>
    public long? AllocationStage2Minor { get; set; }
    public DealPartyStatus Status { get; set; }
    public DateTime? AcceptedAt { get; set; }
}
