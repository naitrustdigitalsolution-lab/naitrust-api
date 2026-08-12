namespace Naitrust.Domain.Models.Dtos.Responses.Public;

public record WaitlistEntryResponse(
    Guid Id,
    string Name,
    string Email,
    string? Phone,
    string? Source,
    string? BusinessName,
    string? UserType,
    string? TransactionRange,
    string? TransactionNeed,
    string? Expectations,
    bool Consent,
    DateTime? SubmittedAt,
    DateTime CreatedAt);
