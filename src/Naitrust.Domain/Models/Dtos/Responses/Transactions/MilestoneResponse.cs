namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record MilestoneResponse(
    Guid Id,
    string Title,
    string? Description,
    long AmountMinor,
    DateTime? DueAt,
    string Status,
    DateTime? SubmittedAt,
    DateTime? ApprovedAt);
