using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Ai;
using Naitrust.Domain.Models.Dtos.Responses.Ai;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get AI risk assessment for a transaction</summary>
    [HttpPost("transactions/{id:guid}/risk-assessment")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<AiAssessmentResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RiskAssessmentAsync(Guid id)
    {
        var response = await _aiService.GetRiskAssessmentAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get AI evidence checklist for a transaction</summary>
    [HttpPost("transactions/{id:guid}/evidence-checklist")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<AiAssessmentResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> EvidenceChecklistAsync(Guid id)
    {
        var response = await _aiService.GetEvidenceChecklistAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get AI dispute summary</summary>
    [HttpPost("disputes/{id:guid}/summary")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<AiAssessmentResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DisputeSummaryAsync(Guid id)
    {
        var response = await _aiService.GetDisputeSummaryAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get AI verification summary</summary>
    [HttpPost("verifications/{id:guid}/summary")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<AiAssessmentResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> VerificationSummaryAsync(Guid id)
    {
        var response = await _aiService.GetVerificationSummaryAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get AI reputation summary for a profile</summary>
    [HttpPost("reputation/{profileId:guid}/summary")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<AiAssessmentResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ReputationSummaryAsync(Guid profileId)
    {
        var response = await _aiService.GetReputationSummaryAsync(profileId);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Admin AI copilot — ask a free-text question about a case</summary>
    [HttpPost("admin/copilot")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<AiAssessmentResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AdminCopilotAsync([FromBody] string query)
    {
        var response = await _aiService.GetAdminCopilotAsync(query);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit feedback on an AI assessment</summary>
    [HttpPost("feedback")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> FeedbackAsync([FromBody] AiFeedbackRequest request)
    {
        var response = await _aiService.SubmitFeedbackAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
