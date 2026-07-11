namespace Naitrust.Domain.Models.Dtos.Responses.Users;

public record UserResponse(
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
    DateTime? LastLivenessVerifiedAt,
    DateTime? LastTransactionActivityAt,
    DateTime CreatedAt);
