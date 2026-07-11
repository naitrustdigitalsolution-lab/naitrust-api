namespace Naitrust.Domain.Models.Dtos.Responses.Auth;

public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
