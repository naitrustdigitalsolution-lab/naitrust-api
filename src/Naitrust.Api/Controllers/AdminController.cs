using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Admin;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "RequireAdmin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// List all transactions (admin)
    /// </summary>
    [HttpGet("transactions")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListTransactionsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetTransactionsAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get a transaction by ID (admin)
    /// </summary>
    [HttpGet("transactions/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetTransactionAsync(Guid id)
    {
        var response = await _adminService.GetTransactionAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// List all disputes (admin)
    /// </summary>
    [HttpGet("disputes")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListDisputesAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetDisputesAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Resolve a dispute (admin)
    /// </summary>
    [HttpPatch("disputes/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ResolveDisputeAsync(Guid id, [FromBody] ResolveAdminDisputeRequest request)
    {
        var response = await _adminService.ResolveDisputeAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// List all verifications (admin)
    /// </summary>
    [HttpGet("verifications")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListVerificationsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetVerificationsAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Update a verification request (admin)
    /// </summary>
    [HttpPatch("verifications/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateVerificationAsync(Guid id, [FromBody] UpdateAdminVerificationRequest request)
    {
        var response = await _adminService.UpdateVerificationAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get system audit logs (admin)
    /// </summary>
    [HttpGet("audit-logs")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAuditLogsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetAuditLogsAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }
}
