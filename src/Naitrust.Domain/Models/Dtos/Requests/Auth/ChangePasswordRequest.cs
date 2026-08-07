namespace Naitrust.Domain.Models.Dtos.Requests.Auth;

public record ChangePasswordRequest(string OldPassword, string NewPassword);
