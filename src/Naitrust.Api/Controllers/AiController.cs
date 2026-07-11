using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Ai;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AiController : ControllerBase
{
    [HttpPost("transactions/{id:guid}/risk-assessment")]
    public Task<IActionResult> RiskAssessmentAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("transactions/{id:guid}/evidence-checklist")]
    public Task<IActionResult> EvidenceChecklistAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("disputes/{id:guid}/summary")]
    public Task<IActionResult> DisputeSummaryAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("verifications/{id:guid}/summary")]
    public Task<IActionResult> VerificationSummaryAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("reputation/{profileId:guid}/summary")]
    public Task<IActionResult> ReputationSummaryAsync(Guid profileId)
        => throw new NotImplementedException();

    [HttpPost("admin/cases/{id:guid}/copilot")]
    public Task<IActionResult> AdminCopilotAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("feedback")]
    public Task<IActionResult> FeedbackAsync([FromBody] AiFeedbackRequest request)
        => throw new NotImplementedException();
}
