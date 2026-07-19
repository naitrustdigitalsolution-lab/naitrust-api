using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class DisputesController : ControllerBase
{
    private readonly IDisputeService _disputeService;

    public DisputesController(IDisputeService disputeService)
    {
        _disputeService = disputeService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Open a dispute against a transaction
    /// </summary>
    [HttpPost("transactions/{id:guid}/disputes")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> OpenAsync(Guid id, [FromBody] OpenDisputeRequest request)
    {
        var response = await _disputeService.OpenDisputeAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// List disputes for a transaction
    /// </summary>
    [HttpGet("transactions/{id:guid}/disputes")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListByTransactionAsync(Guid id)
    {
        var response = await _disputeService.ListDisputesByTransactionAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get a dispute by ID
    /// </summary>
    [HttpGet("disputes/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _disputeService.GetDisputeAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Add a message to a dispute
    /// </summary>
    [HttpPost("disputes/{id:guid}/messages")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddMessageAsync(Guid id, [FromBody] AddDisputeMessageRequest request)
    {
        var response = await _disputeService.AddMessageAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Add evidence to a dispute
    /// </summary>
    [HttpPost("disputes/{id:guid}/evidence")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddEvidenceAsync(Guid id, [FromBody] AddDisputeEvidenceRequest request)
    {
        var response = await _disputeService.AddEvidenceAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
