namespace Naitrust.Domain.Models.Dtos.Requests.Beneficiaries;

public record CreateBeneficiaryRequest(
    string Type,
    string Name,
    string? Email = null,
    string? Phone = null,
    string? NaitrustIdentifier = null,
    string? NaitrustAccountNumber = null,
    string? NaitrustId = null,
    string? BankName = null,
    string? AccountNumber = null);
