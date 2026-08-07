using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions/{txnId:guid}/termination")]
[Authorize]
public class TerminationController : ControllerBase
{
    private readonly IDealSubResourceService _service;

    public TerminationController(IDealSubResourceService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Get the termination request for a transaction
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAsync(Guid txnId)
    {
        var response = await _service.GetTerminationAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Request termination of a transaction
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RequestAsync(Guid txnId, [FromBody] RequestTerminationRequest request)
    {
        var response = await _service.RequestTerminationAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Respond to a termination request (approve or reject)
    /// </summary>
    [HttpPost("respond")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RespondAsync(Guid txnId, [FromBody] RespondTerminationRequest request)
    {
        var response = await _service.RespondToTerminationAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
