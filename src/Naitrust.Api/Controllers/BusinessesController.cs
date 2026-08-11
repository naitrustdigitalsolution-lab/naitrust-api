using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;
using Naitrust.Domain.Models.Dtos.Responses.Businesses;

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

    /// <summary>Create a new business profile for the authenticated user</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<BusinessResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBusinessRequest request)
    {
        var response = await _businessService.CreateBusinessAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get all businesses owned by the authenticated user</summary>
    [HttpGet("me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<BusinessResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMyAsync()
    {
        var response = await _businessService.GetMyBusinessesAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get all businesses owned by the authenticated user (alias)</summary>
    [HttpGet("my/businesses")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<BusinessResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMyBusinessesAliasAsync()
    {
        var response = await _businessService.GetMyBusinessesAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a business by its ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<BusinessResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _businessService.GetBusinessAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Update a business profile</summary>
    [HttpPatch("{id:guid}")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<BusinessResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBusinessRequest request)
    {
        var response = await _businessService.UpdateBusinessAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Add a member to a business</summary>
    [HttpPost("{id:guid}/members")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<BusinessMemberResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddMemberAsync(Guid id, [FromBody] AddBusinessMemberRequest request)
    {
        var response = await _businessService.AddMemberAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Update a business member's role or permissions</summary>
    [HttpPatch("{id:guid}/members/{memberId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<BusinessMemberResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateMemberAsync(Guid id, Guid memberId, [FromBody] UpdateBusinessMemberRequest request)
    {
        var response = await _businessService.UpdateMemberAsync(id, memberId, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Search businesses by name or keyword</summary>
    [HttpGet("search")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<BusinessResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SearchAsync([FromQuery] string q = "")
    {
        var response = await _businessService.SearchAsync(q);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a business's public profile by slug or ID (no auth required)</summary>
    [HttpGet("public/{slugOrId}")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<BusinessResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetPublicProfileAsync(string slugOrId)
    {
        var response = await _businessService.GetPublicProfileAsync(slugOrId);
        return StatusCode((int)response.StatusCode, response);
    }
}
