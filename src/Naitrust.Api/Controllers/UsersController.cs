using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Users;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    public Task<IActionResult> GetMeAsync()
        => throw new NotImplementedException();

    [HttpPatch("me")]
    public Task<IActionResult> UpdateMeAsync([FromBody] UpdateUserRequest request)
        => throw new NotImplementedException();
}
