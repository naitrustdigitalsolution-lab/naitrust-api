namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record RegisterRequest(string Email, string Password, string FirstName, string LastName, string? Phone);
