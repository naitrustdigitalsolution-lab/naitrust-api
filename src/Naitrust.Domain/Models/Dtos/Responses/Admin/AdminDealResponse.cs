namespace Naitrust.Domain.Models.Dtos.Responses.Admin;

public record AdminDealResponse(
    Guid Id,
    string Reference,
    string Title,
    long AmountMinor,
    string Currency,
    string Status,
    string? RiskLevel,
    Guid CreatedByUserId,
    DateTime CreatedAt);
