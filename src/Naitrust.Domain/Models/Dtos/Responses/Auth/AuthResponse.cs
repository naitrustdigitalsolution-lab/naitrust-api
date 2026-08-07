namespace Naitrust.Domain.Models.Dtos.Responses.Auth;

public record AuthResponse(FrontendUserResponse User, string Token, string? RefreshToken = null, bool Requires2FA = false);
