namespace Naitrust.Domain.Models.Dtos.Requests.InstantTransfers;

public record ValidateRecipientRequest(
    string Method,
    string Identifier,
    string? BankName = null,
    string? NaitrustAccountNumber = null,
    string? NaitrustId = null,
    string? AccountType = null);
