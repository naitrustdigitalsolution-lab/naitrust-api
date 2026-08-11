namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Role,
    string? BusinessName,
    PendingBusinessDataInput? PendingBusinessData);

public record PendingBusinessDataInput(
    string Name,
    string? Category,
    string? RegistrationNumber,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? Description,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? SocialHandles,
    string? VerificationType);
