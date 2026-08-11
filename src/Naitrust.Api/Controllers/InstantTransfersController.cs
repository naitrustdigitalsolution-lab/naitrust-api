using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.InstantTransfers;
using Naitrust.Domain.Models.Dtos.Responses.InstantTransfers;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/instant-transfers")]
[Authorize]
public class InstantTransfersController : ControllerBase
{
    private readonly IInstantTransferService _service;

    public InstantTransfersController(IInstantTransferService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Validate a recipient before sending an instant transfer</summary>
    [HttpPost("validate-recipient")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<ValidateRecipientResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ValidateRecipientAsync([FromBody] ValidateRecipientRequest request)
    {
        var response = await _service.ValidateRecipientAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Create and send an instant transfer</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<InstantTransferResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateInstantTransferRequest request)
    {
        var response = await _service.CreateAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a specific instant transfer by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<InstantTransferResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _service.GetByIdAsync(GetUserId(), id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>List all instant transfers for the authenticated user</summary>
    [HttpGet("my")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<InstantTransferResponse>>))]
    public async Task<IActionResult> GetMyAsync()
    {
        var response = await _service.GetMyAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
