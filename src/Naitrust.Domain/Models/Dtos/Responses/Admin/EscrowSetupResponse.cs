namespace Naitrust.Domain.Models.Dtos.Responses.Admin;

public record EscrowSetupResponse(
    Guid VirtualAccountId,
    string ProviderReference,
    string AccountNumber,
    string AccountName,
    string BankName,
    string Status,
    DateTime CreatedAt);
