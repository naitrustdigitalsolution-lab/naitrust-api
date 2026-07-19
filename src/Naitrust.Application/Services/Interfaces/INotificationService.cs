using Naitrust.Domain.Models.Dtos.Responses.Notifications;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Creates and sends a notification to a user (persisted and optionally pushed via SignalR).
    /// </summary>
    Task<NaitrustResponse<NotificationResponse>> SendNotificationAsync(Guid userId, string title, string body, string type, string? metadata = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves paginated notifications for the authenticated user.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<NotificationResponse>>> GetUserNotificationsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Marks a single notification as read for the authenticated user.
    /// </summary>
    Task<NaitrustResponse<bool>> MarkReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Marks all unread notifications as read for the authenticated user.
    /// </summary>
    Task<NaitrustResponse<bool>> MarkAllReadAsync(Guid userId, CancellationToken ct = default);
}
