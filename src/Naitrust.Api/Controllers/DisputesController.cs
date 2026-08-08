using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;

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

    /// <summary>Get the dispute for a transaction</summary>
    [HttpGet("transactions/{txnId:guid}/dispute")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DisputeResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByTransactionAsync(Guid txnId)
    {
        var response = await _disputeService.GetByTransactionAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Open a dispute on a transaction</summary>
    [HttpPost("transactions/{txnId:guid}/dispute")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<DisputeResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> OpenAsync(Guid txnId, [FromBody] OpenDisputeRequest request)
    {
        var response = await _disputeService.OpenDisputeAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Add a message to a dispute thread</summary>
    [HttpPost("transactions/{txnId:guid}/dispute/messages")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<DisputeResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddMessageAsync(Guid txnId, [FromBody] AddDisputeMessageRequest request)
    {
        var response = await _disputeService.AddMessageToTransactionDisputeAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
