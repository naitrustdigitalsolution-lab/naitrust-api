using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Roles;
using Naitrust.Domain.Models.Dtos.Responses.Roles;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/role-claims")]
[Authorize(Policy = "RequireAdmin")]
public class RoleClaimsController : ControllerBase
{
    private readonly IRoleClaimService _roleClaimService;

    public RoleClaimsController(IRoleClaimService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }

    /// <summary>Get all claims assigned to a role</summary>
    [HttpGet("{role}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<RoleClaimResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetClaimsByRoleAsync(string role)
    {
        var response = await _roleClaimService.GetClaimsByRoleAsync(role);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Add a claim to a role</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddClaimAsync([FromBody] RoleClaimRequest request)
    {
        var response = await _roleClaimService.AddClaimAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Update a claim on a role</summary>
    [HttpPut]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateClaimAsync([FromBody] UpdateRoleClaimRequest request)
    {
        var response = await _roleClaimService.UpdateClaimAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Remove a claim from a role</summary>
    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RemoveClaimAsync([FromBody] RoleClaimRequest request)
    {
        var response = await _roleClaimService.RemoveClaimAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
