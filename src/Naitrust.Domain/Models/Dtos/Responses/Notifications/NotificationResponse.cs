namespace Naitrust.Domain.Models.Dtos.Responses.Notifications;

public record NotificationResponse(
    Guid Id,
    string Type,
    string Title,
    string Body,
    string? Metadata,
    DateTime? ReadAt,
    DateTime CreatedAt);
