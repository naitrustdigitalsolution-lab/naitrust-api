namespace Naitrust.Domain.Models.Dtos.Responses.Roles;

public record RoleResponse(Guid Id, string Name, string? Description, DateTime CreatedAt);
