namespace Naitrust.Domain.Models.Dtos.Responses.Security;

public record Start2faSetupResponse(string Secret, string OtpauthUri);
