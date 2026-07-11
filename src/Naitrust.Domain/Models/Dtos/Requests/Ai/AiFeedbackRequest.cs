namespace Naitrust.Domain.Models.Dtos.Requests.Ai;

public record AiFeedbackRequest(Guid AssessmentId, string FeedbackType, string? Notes);
