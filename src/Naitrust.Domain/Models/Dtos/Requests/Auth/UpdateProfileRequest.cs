namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record UpdateProfileRequest(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Bio,
    string? Address,
    string? City,
    string? State,
    string? Country);
