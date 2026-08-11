using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Beneficiaries;
using Naitrust.Domain.Models.Dtos.Responses.Beneficiaries;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/beneficiaries")]
[Authorize]
public class BeneficiariesController : ControllerBase
{
    private readonly IBeneficiaryService _service;

    public BeneficiariesController(IBeneficiaryService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>List all saved beneficiaries for the authenticated user</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<BeneficiaryResponse>>))]
    public async Task<IActionResult> ListAsync()
    {
        var response = await _service.ListAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Save a new beneficiary</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<BeneficiaryResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBeneficiaryRequest request)
    {
        var response = await _service.CreateAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Remove a saved beneficiary</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var response = await _service.DeleteAsync(GetUserId(), id);
        return StatusCode((int)response.StatusCode, response);
    }
}
