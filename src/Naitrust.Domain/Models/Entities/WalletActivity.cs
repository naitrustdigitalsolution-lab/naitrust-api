namespace Naitrust.Domain.Models.Entities;

public class WalletActivity : BaseEntity
{
    public Guid UserId { get; set; }
    /// <summary>withdrawal | funding | protected_allocation | protected_release | instant_transfer_out | instant_transfer_in | fee</summary>
    public string Kind { get; set; } = default!;
    public long AmountMinor { get; set; }
    public string Currency { get; set; } = "NGN";
    public string Description { get; set; } = default!;
}
