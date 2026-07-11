namespace Naitrust.Domain.Models.Dtos.Responses.Verification;

public record VerificationStepResponse(
    string Step,
    string Status,
    string? Message,
    DateTime? StartedAt,
    DateTime? CompletedAt);
