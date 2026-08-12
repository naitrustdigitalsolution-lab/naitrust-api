using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Users;
using Naitrust.Domain.Models.Dtos.Responses.Users;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get the current user's profile</summary>
    [HttpGet("me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<UserResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMeAsync()
    {
        var response = await _userService.GetUserAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Update the current user's profile</summary>
    [HttpPatch("me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<UserResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateMeAsync([FromBody] UpdateUserRequest request)
    {
        var response = await _userService.UpdateUserAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
