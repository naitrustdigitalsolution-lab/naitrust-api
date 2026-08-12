namespace Naitrust.Domain.Models.Dtos.Responses.Auth;

public record FrontendUserResponse(
    Guid Id,
    string Email,
    string? FirstName,
    string? LastName,
    string Name,
    string Role,
    string? Phone,
    string? Avatar,
    int? KycLevel,
    bool? KycVerified,
    bool? IsEmailVerified,
    bool? IsPhoneVerified);
