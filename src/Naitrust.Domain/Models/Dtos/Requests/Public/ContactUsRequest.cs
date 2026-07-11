namespace Naitrust.Domain.Models.Dtos.Requests.Public;

public record ContactUsRequest(string Name, string Email, string Subject, string Message);
