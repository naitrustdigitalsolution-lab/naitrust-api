namespace Naitrust.Domain.Models.Dtos.Responses.Auth;

public record UserProfileResponse(
    Guid Id,
    string Email,
    string? Phone,
    string FirstName,
    string LastName,
    string Role,
    string Status,
    DateTime? EmailVerifiedAt,
    DateTime? PhoneVerifiedAt,
    DateTime? IdentityVerifiedAt,
    DateTime? LastLivenessVerifiedAt);
