using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Invitations;
using Naitrust.Domain.Models.Dtos.Responses.Invitations;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/invitations")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationsController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>List all deal invitations for the authenticated user</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<InvitationListItemResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListAsync()
    {
        var response = await _invitationService.ListAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a specific invitation by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealInvitationResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await _invitationService.GetAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Accept a deal invitation</summary>
    [HttpPost("{id:guid}/accept")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealInvitationResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AcceptAsync(Guid id)
    {
        var response = await _invitationService.RespondAsync(id, GetUserId(), "accepted");
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Decline a deal invitation</summary>
    [HttpPost("{id:guid}/decline")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealInvitationResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DeclineAsync(Guid id)
    {
        var response = await _invitationService.RespondAsync(id, GetUserId(), "declined");
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a public preview of an invitation by its share token (no auth required)</summary>
    [HttpGet("public/{token}")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PublicInvitationPreviewResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetPublicPreviewAsync(string token)
    {
        var response = await _invitationService.GetPublicPreviewAsync(token);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Claim an invitation using a public share token (links a user account to the deal)</summary>
    [HttpPost("public/{token}/claim")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealInvitationResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ClaimAsync(string token, [FromBody] ClaimInvitationRequest request)
    {
        var response = await _invitationService.ClaimAsync(token, request);
        return StatusCode((int)response.StatusCode, response);
    }
}
