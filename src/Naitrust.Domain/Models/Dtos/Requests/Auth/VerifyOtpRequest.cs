namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record VerifyOtpRequest(string Email, string Otp);
