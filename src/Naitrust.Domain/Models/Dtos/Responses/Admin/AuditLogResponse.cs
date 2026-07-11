namespace Naitrust.Domain.Models.Dtos.Responses.Admin;

public record AuditLogResponse(
    Guid Id,
    Guid? ActorUserId,
    string Action,
    string EntityType,
    Guid EntityId,
    string? IpAddress,
    DateTime CreatedAt);
