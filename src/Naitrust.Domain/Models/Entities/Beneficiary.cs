namespace Naitrust.Domain.Models.Entities;

public class Beneficiary : BaseEntity
{
    public Guid UserId { get; set; }
    public string Type { get; set; } = default!; // naitrust_user | bank_account
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? NaitrustIdentifier { get; set; }
    public string? NaitrustAccountNumber { get; set; }
    public string? NaitrustId { get; set; }
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public bool IsFavourite { get; set; }
}
