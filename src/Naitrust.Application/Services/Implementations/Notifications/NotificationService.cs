using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Notifications;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Notifications;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<NotificationResponse>> SendNotificationAsync(Guid userId, string title, string body, string type, string? metadata = null, CancellationToken ct = default)
    {
        if (!Enum.TryParse<NotificationType>(type, ignoreCase: true, out var notificationType))
        {
            return NaitrustResponse<NotificationResponse>.BadRequest($"Invalid notification type: {type}");
        }

        var repo = _unitOfWork.GetRepository<Notification>();

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = notificationType,
            Title = title,
            Body = body,
            Metadata = metadata,
            IsActive = true
        };

        await repo.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        // TODO: Push notification via SignalR

        return NaitrustResponse<NotificationResponse>.Created(
            "Notification sent successfully.",
            MapToResponse(notification));
    }

    public async Task<NaitrustResponse<PaginatedResponse<NotificationResponse>>> GetUserNotificationsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Notification>();

        var allNotifications = await repo.GetAllDataAsync(
            n => n.UserId == userId && !n.IsDeleted,
            orderBy: q => q.OrderByDescending(n => n.CreatedAt));

        var notificationList = allNotifications.ToList();
        var totalCount = notificationList.Count;

        var pagedNotifications = notificationList
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var responses = pagedNotifications.Select(MapToResponse).ToList();
        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<NotificationResponse>>.Success(
            "Notifications retrieved successfully.",
            new PaginatedResponse<NotificationResponse>(responses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<bool>> MarkReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Notification>();
        var notification = await repo.GetSingleByAsync(n => n.Id == notificationId && n.UserId == userId && !n.IsDeleted);

        if (notification is null)
        {
            return NaitrustResponse<bool>.NotFound("Notification not found.");
        }

        notification.ReadAt = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Notification marked as read.", true);
    }

    public async Task<NaitrustResponse<bool>> MarkAllReadAsync(Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Notification>();
        var unreadNotifications = await repo.GetAllDataAsync(
            n => n.UserId == userId && n.ReadAt == null && !n.IsDeleted);

        var unreadList = unreadNotifications.ToList();

        if (unreadList.Count == 0)
        {
            return NaitrustResponse<bool>.Success("No unread notifications found.", true);
        }

        var now = DateTime.UtcNow;
        foreach (var notification in unreadList)
        {
            notification.ReadAt = now;
            notification.UpdatedAt = now;
        }

        await repo.UpdateRangeAsync(unreadList);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("All notifications marked as read.", true);
    }

    private static NotificationResponse MapToResponse(Notification notification)
    {
        return new NotificationResponse(
            notification.Id,
            notification.Type.ToString(),
            notification.Title,
            notification.Body,
            notification.Metadata,
            notification.ReadAt,
            notification.CreatedAt);
    }
}
