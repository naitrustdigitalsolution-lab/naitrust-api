using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> ListAsync([FromQuery] PaginationRequest pagination)
        => throw new NotImplementedException();

    [HttpPatch("{id:guid}/read")]
    public Task<IActionResult> MarkReadAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("read-all")]
    public Task<IActionResult> MarkAllReadAsync()
        => throw new NotImplementedException();
}
