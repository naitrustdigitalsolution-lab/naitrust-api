using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.TrustProfile;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/trust-profile")]
[Authorize]
public class TrustProfileController : ControllerBase
{
    private readonly ITrustProfileService _service;

    public TrustProfileController(ITrustProfileService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get the authenticated user's trust profile summary</summary>
    [HttpGet("me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<TrustProfileResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMyAsync()
    {
        var response = await _service.GetMyAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
