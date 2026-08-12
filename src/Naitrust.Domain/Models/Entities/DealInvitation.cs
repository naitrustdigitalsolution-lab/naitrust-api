using Naitrust.Domain.Models.Enums.Transactions;

namespace Naitrust.Domain.Models.Entities;

public class DealInvitation : BaseEntity
{
    public Guid DealId { get; set; }
    public string PublicToken { get; set; } = default!;
    public Guid? RecipientUserId { get; set; }
    public string? IntendedContact { get; set; }
    public string? IntendedAccountType { get; set; }
    public Guid? InviterProfileId { get; set; }
    public Guid? InviteeProfileId { get; set; }
    public string? PostAuthDestination { get; set; }
    public string FromName { get; set; } = default!;
    public string FromRole { get; set; } = default!;
    public string YourRole { get; set; } = default!;
    public string PartyMode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public long AmountMinor { get; set; }
    public string Currency { get; set; } = default!;
    public string? Message { get; set; }
    public string? AgreementSnapshot { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime ExpiresAt { get; set; }
}
