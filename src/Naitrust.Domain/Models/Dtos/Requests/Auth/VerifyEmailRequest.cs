namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record VerifyEmailRequest(string Email, string Otp);
