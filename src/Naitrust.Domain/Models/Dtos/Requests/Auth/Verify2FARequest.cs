namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record Verify2FARequest(Guid? UserId, string? Email, string Token);
