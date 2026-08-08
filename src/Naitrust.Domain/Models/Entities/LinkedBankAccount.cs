namespace Naitrust.Domain.Models.Entities;

public class LinkedBankAccount : BaseEntity
{
    public Guid UserId { get; set; }
    public string BankCode { get; set; } = default!;
    public string BankName { get; set; } = default!;
    public string AccountNumber { get; set; } = default!;
    public string AccountName { get; set; } = default!;
    public bool IsDefault { get; set; }
    public DateTime? VerifiedAt { get; set; }
}
