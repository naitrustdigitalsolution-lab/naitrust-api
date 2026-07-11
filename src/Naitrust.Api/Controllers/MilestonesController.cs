using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions/{transactionId:guid}/milestones")]
[Authorize]
public class MilestonesController : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> CreateAsync(Guid transactionId, [FromBody] CreateMilestoneRequest request)
        => throw new NotImplementedException();

    [HttpPatch("{milestoneId:guid}")]
    public Task<IActionResult> UpdateAsync(Guid transactionId, Guid milestoneId, [FromBody] UpdateMilestoneRequest request)
        => throw new NotImplementedException();

    [HttpPost("{milestoneId:guid}/submit")]
    public Task<IActionResult> SubmitAsync(Guid transactionId, Guid milestoneId)
        => throw new NotImplementedException();

    [HttpPost("{milestoneId:guid}/approve")]
    public Task<IActionResult> ApproveAsync(Guid transactionId, Guid milestoneId)
        => throw new NotImplementedException();
}
