namespace Naitrust.Domain.Models.Dtos.Responses.Businesses;

public record BusinessResponse(
    Guid Id,
    Guid OwnerUserId,
    string Name,
    string Type,
    string? RegistrationNumber,
    string? TaxId,
    string Country,
    string? State,
    string? Address,
    string VerificationStatus,
    string? RiskLevel,
    DateTime? BusinessVerifiedAt,
    DateTime? OwnershipVerifiedAt,
    DateTime CreatedAt);
