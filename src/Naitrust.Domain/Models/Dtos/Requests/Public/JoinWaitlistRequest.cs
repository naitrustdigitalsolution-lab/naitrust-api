namespace Naitrust.Domain.Models.Dtos.Requests.Public;

public record JoinWaitlistRequest(
    string Name,
    string Email,
    string? Phone,
    string? Source,
    string? BusinessName,
    string? UserType,
    string? TransactionRange,
    string? TransactionNeed,
    string? Expectations,
    bool Consent = true,
    DateTime? SubmittedAt = null);
