namespace Naitrust.Domain.Models.Dtos.Responses.Reputation;

public record ReviewResponse(
    Guid Id,
    Guid TransactionId,
    Guid ReviewerUserId,
    int Rating,
    string? Comment,
    DateTime CreatedAt);
