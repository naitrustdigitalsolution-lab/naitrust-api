using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// List the current user's notifications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _notificationService.GetUserNotificationsAsync(GetUserId(), pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Mark a notification as read
    /// </summary>
    [HttpPatch("{id:guid}/read")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> MarkReadAsync(Guid id)
    {
        var response = await _notificationService.MarkReadAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Mark all notifications as read
    /// </summary>
    [HttpPost("read-all")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> MarkAllReadAsync()
    {
        var response = await _notificationService.MarkAllReadAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
