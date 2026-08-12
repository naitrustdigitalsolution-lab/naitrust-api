namespace Naitrust.Domain.Models.Entities;

public class InstantTransfer : BaseEntity
{
    public Guid UserId { get; set; }
    public string Reference { get; set; } = default!;

    // Recipient
    public string RecipientMethod { get; set; } = default!;
    public string RecipientIdentifier { get; set; } = default!;
    public string? RecipientResolvedName { get; set; }
    public string? RecipientBankName { get; set; }
    public string? RecipientNaitrustAccountNumber { get; set; }
    public string? RecipientNaitrustId { get; set; }
    public string? RecipientAccountType { get; set; }
    public bool RecipientIdentityVerified { get; set; }

    // Transfer details
    public long AmountMinor { get; set; }
    public string Currency { get; set; } = "NGN";
    public long FeeMinor { get; set; }
    public string? Narration { get; set; }
    public string Status { get; set; } = "draft";
    public string Provider { get; set; } = "mock";
    public bool IsMock { get; set; } = true;
    public DateTime? CompletedAt { get; set; }
}
