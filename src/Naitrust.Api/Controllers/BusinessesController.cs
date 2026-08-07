using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/businesses")]
[Authorize]
public class BusinessesController : ControllerBase
{
    private readonly IBusinessService _businessService;

    public BusinessesController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBusinessRequest request)
    {
        var response = await _businessService.CreateBusinessAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyAsync()
    {
        var response = await _businessService.GetMyBusinessesAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("my/businesses")]
    public async Task<IActionResult> GetMyBusinessesAliasAsync()
    {
        var response = await _businessService.GetMyBusinessesAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _businessService.GetBusinessAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBusinessRequest request)
    {
        var response = await _businessService.UpdateBusinessAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("{id:guid}/members")]
    public async Task<IActionResult> AddMemberAsync(Guid id, [FromBody] AddBusinessMemberRequest request)
    {
        var response = await _businessService.AddMemberAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPatch("{id:guid}/members/{memberId:guid}")]
    public async Task<IActionResult> UpdateMemberAsync(Guid id, Guid memberId, [FromBody] UpdateBusinessMemberRequest request)
    {
        var response = await _businessService.UpdateMemberAsync(id, memberId, request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync([FromQuery] string q = "")
    {
        var response = await _businessService.SearchAsync(q);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("public/{slugOrId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicProfileAsync(string slugOrId)
    {
        var response = await _businessService.GetPublicProfileAsync(slugOrId);
        return StatusCode((int)response.StatusCode, response);
    }
}
