using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Notifications;

namespace Naitrust.Application.Services.Implementations.Notifications;

public class NotificationService : INotificationService
{
    public Task<NaitrustResponse<NotificationResponse>> SendNotificationAsync(Guid userId, string title, string body, string type, string? metadata = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<NotificationResponse>>> GetUserNotificationsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> MarkReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> MarkAllReadAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
