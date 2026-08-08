using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Negotiations;
using Naitrust.Domain.Models.Dtos.Responses.Negotiations;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions/{txnId:guid}/negotiation")]
[Authorize]
public class NegotiationsController : ControllerBase
{
    private readonly INegotiationService _negotiationService;

    public NegotiationsController(INegotiationService negotiationService)
    {
        _negotiationService = negotiationService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get the current negotiation for a transaction</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<NegotiationResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAsync(Guid txnId)
    {
        var response = await _negotiationService.GetByTransactionAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Propose changes to the deal terms (opens or counters a negotiation)</summary>
    [HttpPost("propose")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<NegotiationResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ProposeAsync(Guid txnId, [FromBody] ProposeNegotiationRequest request)
    {
        var response = await _negotiationService.ProposeAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Accept or decline a specific negotiation proposal</summary>
    [HttpPost("proposals/{proposalId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<NegotiationResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RespondToProposalAsync(Guid txnId, Guid proposalId, [FromBody] RespondToProposalRequest request)
    {
        var response = await _negotiationService.RespondToProposalAsync(txnId, proposalId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Withdraw the entire negotiation</summary>
    [HttpPost("withdraw")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<NegotiationResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> WithdrawAsync(Guid txnId)
    {
        var response = await _negotiationService.WithdrawAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
