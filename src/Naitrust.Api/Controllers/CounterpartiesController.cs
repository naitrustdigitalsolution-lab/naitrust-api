using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Counterparties;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/counterparties")]
[Authorize]
public class CounterpartiesController : ControllerBase
{
    private readonly ICounterpartyService _service;

    public CounterpartiesController(ICounterpartyService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>List all counterparties (people/businesses from past deals)</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<CounterpartyProfileResponse>>))]
    public async Task<IActionResult> ListAsync()
    {
        var response = await _service.ListAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Toggle favourite status for a counterparty</summary>
    [HttpPost("{id:guid}/favourite")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<CounterpartyProfileResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ToggleFavouriteAsync(Guid id)
    {
        var response = await _service.ToggleFavouriteAsync(GetUserId(), id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Toggle blocked status for a counterparty</summary>
    [HttpPost("{id:guid}/block")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<CounterpartyProfileResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ToggleBlockAsync(Guid id)
    {
        var response = await _service.ToggleBlockAsync(GetUserId(), id);
        return StatusCode((int)response.StatusCode, response);
    }
}
