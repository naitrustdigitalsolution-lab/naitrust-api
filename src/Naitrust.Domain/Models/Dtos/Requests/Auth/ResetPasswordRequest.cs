namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record ResetPasswordRequest(string Email, string ResetToken, string NewPassword);
