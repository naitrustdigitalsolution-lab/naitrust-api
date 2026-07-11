using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

public class TransactionParty : BaseEntity
{
    public Guid TransactionId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? BusinessId { get; set; }
    public PartyType PartyType { get; set; }
    public PartyMode PartyMode { get; set; }
    public string DisplayName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public TransactionPartyStatus Status { get; set; }
    public DateTime? AcceptedAt { get; set; }
}
