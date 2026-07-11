using Naitrust.Domain.Models.Dtos.Responses.Notifications;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface INotificationService
{
    Task<NaitrustResponse<NotificationResponse>> SendNotificationAsync(Guid userId, string title, string body, string type, string? metadata = null, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<NotificationResponse>>> GetUserNotificationsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> MarkReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> MarkAllReadAsync(Guid userId, CancellationToken ct = default);
}
