namespace Naitrust.Domain.Models.Dtos.Requests.Public;

public record SubmitFeedbackRequest(string? Name, string? Email, int Rating, string? Message);
