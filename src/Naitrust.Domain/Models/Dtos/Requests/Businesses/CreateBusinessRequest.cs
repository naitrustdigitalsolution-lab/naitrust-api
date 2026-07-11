namespace Naitrust.Domain.Models.Dtos.Requests.Businesses;

public record CreateBusinessRequest(
    string Name,
    string Type,
    string? RegistrationNumber,
    string? TaxId,
    string Country,
    string? State,
    string? Address);
