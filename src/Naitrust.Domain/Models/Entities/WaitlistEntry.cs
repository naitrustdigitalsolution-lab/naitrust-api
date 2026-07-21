namespace Naitrust.Domain.Models.Entities;

public class WaitlistEntry : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Source { get; set; }
    public string? BusinessName { get; set; }
    public string? UserType { get; set; }
    public string? TransactionRange { get; set; }
    public string? TransactionNeed { get; set; }
    public string? Expectations { get; set; }
    public bool Consent { get; set; }
    public DateTime? SubmittedAt { get; set; }
}
