namespace Naitrust.Domain.Models.Entities;

public class PaymentRequest : BaseEntity
{
    public Guid RequestedByUserId { get; set; }
    public string Reference { get; set; } = default!;
    public string RequestedFromName { get; set; } = default!;
    public long AmountMinor { get; set; }
    public string Currency { get; set; } = "NGN";
    public string? Reason { get; set; }
    public string Status { get; set; } = "pending"; // pending, fulfilled, declined, expired, cancelled
    public DateTime ExpiresAt { get; set; }
}
