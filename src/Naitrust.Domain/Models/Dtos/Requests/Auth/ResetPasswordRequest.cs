namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record ResetPasswordRequest(string Token, string NewPassword);
