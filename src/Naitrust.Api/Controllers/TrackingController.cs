using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions/{txnId:guid}/tracking")]
[Authorize]
public class TrackingController : ControllerBase
{
    private readonly IDealSubResourceService _service;

    public TrackingController(IDealSubResourceService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get tracking milestones for a transaction</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<TrackingMilestoneResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAsync(Guid txnId)
    {
        var response = await _service.GetTrackingAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Add a custom tracking step to a transaction</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<List<TrackingMilestoneResponse>>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddStepAsync(Guid txnId, [FromBody] AddTrackingStepRequest request)
    {
        var response = await _service.AddTrackingStepAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Advance tracking to the next milestone</summary>
    [HttpPost("advance")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<TrackingMilestoneResponse>>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AdvanceAsync(Guid txnId)
    {
        var response = await _service.AdvanceTrackingAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Revert the last tracking advance</summary>
    [HttpPost("revert")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<TrackingMilestoneResponse>>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RevertAsync(Guid txnId)
    {
        var response = await _service.RevertTrackingAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Edit a tracking step's title or description</summary>
    [HttpPatch("{stepId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<TrackingMilestoneResponse>>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> EditStepAsync(Guid txnId, Guid stepId, [FromBody] EditTrackingStepRequest request)
    {
        var response = await _service.EditTrackingStepAsync(txnId, stepId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
