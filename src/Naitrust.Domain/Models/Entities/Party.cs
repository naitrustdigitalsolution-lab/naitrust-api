namespace Naitrust.Domain.Models.Entities;

public class Party : BaseEntity
{
    public string PartyType { get; set; } = default!;
    public Guid? UserId { get; set; }
    public Guid? BusinessId { get; set; }
    public string DisplayName { get; set; } = default!;
    public string? VerificationStatus { get; set; }
    public string? BvnReference { get; set; }
    public string? CacReference { get; set; }
    public Guid? ReputationProfileId { get; set; }
}
