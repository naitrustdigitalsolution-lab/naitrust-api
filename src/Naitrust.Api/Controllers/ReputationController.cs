using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Reputation;
using Naitrust.Domain.Models.Dtos.Responses.Reputation;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/reputation")]
[Authorize]
public class ReputationController : ControllerBase
{
    private readonly IReputationService _reputationService;

    public ReputationController(IReputationService reputationService)
    {
        _reputationService = reputationService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get a user's reputation profile by profile ID</summary>
    [HttpGet("{profileId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<ReputationProfileResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetProfileAsync(Guid profileId)
    {
        var response = await _reputationService.GetProfileAsync(profileId);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get the current user's reputation profile</summary>
    [HttpGet("me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<ReputationProfileResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMyAsync()
    {
        var response = await _reputationService.GetMyProfileAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit a review for a counterparty on a completed transaction</summary>
    [HttpPost("~/api/transactions/{id:guid}/reviews")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<ReviewResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> PostReviewAsync(Guid id, [FromBody] SubmitReviewRequest request)
    {
        var response = await _reputationService.SubmitReviewAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
