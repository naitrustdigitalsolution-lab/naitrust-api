using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Evidence;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class EvidenceController : ControllerBase
{
    private readonly IEvidenceService _evidenceService;

    public EvidenceController(IEvidenceService evidenceService)
    {
        _evidenceService = evidenceService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Upload evidence for a transaction
    /// </summary>
    [HttpPost("transactions/{id:guid}/evidence")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UploadAsync(Guid id, [FromBody] UploadEvidenceRequest request)
    {
        var response = await _evidenceService.UploadEvidenceAsync(GetUserId(), request, Stream.Null, string.Empty);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// List evidence files for a transaction
    /// </summary>
    [HttpGet("transactions/{id:guid}/evidence")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListByTransactionAsync(Guid id)
    {
        var response = await _evidenceService.ListEvidenceByTransactionAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get evidence by ID
    /// </summary>
    [HttpGet("evidence/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _evidenceService.GetEvidenceAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }
}
