using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Verification;

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

    /// <summary>List all transactions (admin)</summary>
    [HttpGet("transactions")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PaginatedResponse<DealResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListTransactionsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetDealsAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a transaction by ID (admin)</summary>
    [HttpGet("transactions/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetTransactionAsync(Guid id)
    {
        var response = await _adminService.GetDealAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>List all disputes (admin)</summary>
    [HttpGet("disputes")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PaginatedResponse<DisputeResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListDisputesAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetDisputesAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Resolve a dispute with an admin decision (approve, reject, escalate)</summary>
    [HttpPatch("disputes/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DisputeResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ResolveDisputeAsync(Guid id, [FromBody] ResolveAdminDisputeRequest request)
    {
        var response = await _adminService.ResolveDisputeAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>List all verification requests (admin)</summary>
    [HttpGet("verifications")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PaginatedResponse<VerificationRequestResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListVerificationsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetVerificationsAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Update a verification request status (approve, reject, request more info)</summary>
    [HttpPatch("verifications/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateVerificationAsync(Guid id, [FromBody] UpdateAdminVerificationRequest request)
    {
        var response = await _adminService.UpdateVerificationAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Provision the platform's central escrow subledger on Anchor (one-time setup, idempotent)</summary>
    [HttpPost("platform/escrow")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<EscrowSetupResponse>))]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<EscrowSetupResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SetupPlatformEscrowAsync()
    {
        var response = await _adminService.SetupPlatformEscrowAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get system audit logs (admin)</summary>
    [HttpGet("audit-logs")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PaginatedResponse<AuditLogResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAuditLogsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _adminService.GetAuditLogsAsync(pagination);
        return StatusCode((int)response.StatusCode, response);
    }
}
