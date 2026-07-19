using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Roles;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Policy = "RequireAdmin")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        var response = await _roleService.GetAllRolesAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get roles for a user by email
    /// </summary>
    [HttpGet("user/{email}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetUserRolesAsync(string email)
    {
        var response = await _roleService.GetUserRolesAsync(email);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request)
    {
        var response = await _roleService.CreateRoleAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Update an existing role
    /// </summary>
    [HttpPut("{roleId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateRoleAsync(Guid roleId, [FromBody] UpdateRoleRequest request)
    {
        var response = await _roleService.UpdateRoleAsync(roleId, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    [HttpDelete("{roleId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DeleteRoleAsync(Guid roleId)
    {
        var response = await _roleService.DeleteRoleAsync(roleId);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Assign a role to a user
    /// </summary>
    [HttpPost("assign")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AssignRoleAsync([FromBody] AssignRoleRequest request)
    {
        var response = await _roleService.AssignRoleAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Remove a role from a user
    /// </summary>
    [HttpPut("remove")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RemoveFromRoleAsync([FromBody] AssignRoleRequest request)
    {
        var response = await _roleService.RemoveFromRoleAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
