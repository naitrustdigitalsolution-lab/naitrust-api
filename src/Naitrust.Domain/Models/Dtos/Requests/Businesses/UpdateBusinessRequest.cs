namespace Naitrust.Domain.Models.Dtos.Requests.Businesses;

public record UpdateBusinessRequest(string? Name, string? Address, string? State, string? TaxId);
