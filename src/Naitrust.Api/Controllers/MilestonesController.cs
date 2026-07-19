using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Api.Controllers;

/// <summary>
/// Milestone management endpoints (stub — milestone service not yet implemented)
/// </summary>
[ApiController]
[Route("api/transactions/{transactionId:guid}/milestones")]
[Authorize]
public class MilestonesController : ControllerBase
{
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Create a milestone for a transaction
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public Task<IActionResult> CreateAsync(Guid transactionId, [FromBody] CreateMilestoneRequest request)
        => throw new NotImplementedException();

    /// <summary>
    /// Update a milestone
    /// </summary>
    [HttpPatch("{milestoneId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public Task<IActionResult> UpdateAsync(Guid transactionId, Guid milestoneId, [FromBody] UpdateMilestoneRequest request)
        => throw new NotImplementedException();

    /// <summary>
    /// Submit a milestone for approval
    /// </summary>
    [HttpPost("{milestoneId:guid}/submit")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public Task<IActionResult> SubmitAsync(Guid transactionId, Guid milestoneId)
        => throw new NotImplementedException();

    /// <summary>
    /// Approve a submitted milestone
    /// </summary>
    [HttpPost("{milestoneId:guid}/approve")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public Task<IActionResult> ApproveAsync(Guid transactionId, Guid milestoneId)
        => throw new NotImplementedException();
}
