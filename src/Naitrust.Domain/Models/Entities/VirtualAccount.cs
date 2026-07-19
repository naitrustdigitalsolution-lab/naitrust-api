using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Domain.Models.Entities;

public class VirtualAccount : BaseEntity
{
    public Guid? UserId { get; set; }
    public Guid? BusinessId { get; set; }
    public VirtualAccountType Type { get; set; }
    public PaymentPartnerId Partner { get; set; }
    public string? ProviderReference { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public string? BankName { get; set; }
    public long AmountReceivedMinor { get; set; }
    public string Currency { get; set; } = default!;
    public VirtualAccountStatus Status { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? FundedAt { get; set; }
}
