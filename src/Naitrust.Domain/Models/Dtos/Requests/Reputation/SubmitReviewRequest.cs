namespace Naitrust.Domain.Models.Dtos.Requests.Reputation;

public record SubmitReviewRequest(Guid TransactionId, int Rating, string? Comment);
