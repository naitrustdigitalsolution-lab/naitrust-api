using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Domain.Models.Entities;

public class LedgerEntry
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Guid EntryGroupId { get; set; }
    public LedgerEventType EventType { get; set; }
    public string Account { get; set; } = default!;
    public long DebitMinor { get; set; }
    public long CreditMinor { get; set; }
    public string Currency { get; set; } = default!;
    public string? Memo { get; set; }
    public DateTime CreatedAt { get; set; }
}
