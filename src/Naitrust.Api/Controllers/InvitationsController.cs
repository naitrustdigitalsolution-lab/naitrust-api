using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Requests.Invitations;

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

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var response = await _invitationService.ListAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await _invitationService.GetAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<IActionResult> AcceptAsync(Guid id)
    {
        var response = await _invitationService.RespondAsync(id, GetUserId(), "accepted");
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("{id:guid}/decline")]
    public async Task<IActionResult> DeclineAsync(Guid id)
    {
        var response = await _invitationService.RespondAsync(id, GetUserId(), "declined");
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("public/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicPreviewAsync(string token)
    {
        var response = await _invitationService.GetPublicPreviewAsync(token);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("public/{token}/claim")]
    public async Task<IActionResult> ClaimAsync(string token, [FromBody] ClaimInvitationRequest request)
    {
        var response = await _invitationService.ClaimAsync(token, request);
        return StatusCode((int)response.StatusCode, response);
    }
}
