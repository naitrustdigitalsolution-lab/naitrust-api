namespace Naitrust.Domain.Models.Dtos.Requests.Businesses;

public record CreateBusinessRequest(
    string Name,
    string Type,
    string? Slug,
    string? Description,
    string? OwnerName,
    string? Email,
    string? Phone,
    string? PhoneNumber,
    string? Website,
    string? RegistrationNumber,
    string? TaxId,
    string Country,
    string? State,
    string? City,
    string? Address,
    string? SocialHandles,
    string? PaymentAccountBankName,
    string? PaymentAccountNumber,
    string? PaymentAccountName);
