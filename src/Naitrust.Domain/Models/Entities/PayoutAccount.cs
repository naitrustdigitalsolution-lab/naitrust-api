using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Domain.Models.Entities;

public class PayoutAccount : BaseEntity
{
    public Guid PartyId { get; set; }
    public string BankCode { get; set; } = default!;
    public string? BankName { get; set; }
    public string AccountNumberHash { get; set; } = default!;
    public string? AccountName { get; set; }
    public NameMatchStatus NameMatchStatus { get; set; }
    public string? ProviderReference { get; set; }
    public DateTime? VerifiedAt { get; set; }
}
