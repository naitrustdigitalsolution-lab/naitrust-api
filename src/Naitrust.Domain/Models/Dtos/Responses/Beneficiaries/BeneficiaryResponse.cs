namespace Naitrust.Domain.Models.Dtos.Responses.Beneficiaries;

public record BeneficiaryResponse(
    Guid Id,
    string Type,
    string Name,
    string? Email,
    string? Phone,
    string? NaitrustIdentifier,
    string? NaitrustAccountNumber,
    string? NaitrustId,
    string? BankName,
    string? AccountNumber,
    bool IsFavourite,
    DateTime CreatedAt);
