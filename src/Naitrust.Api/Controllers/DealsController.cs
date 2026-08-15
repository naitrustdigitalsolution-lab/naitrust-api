using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Security;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class DealsController : ControllerBase
{
    private readonly IDealService _dealService;
    private readonly IDealOrchestrator _orchestrator;

    public DealsController(IDealService dealService, IDealOrchestrator orchestrator)
    {
        _dealService = dealService;
        _orchestrator = orchestrator;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Create a new deal</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDealRequest request)
    {
        var response = await _dealService.CreateDealAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>List the current user's deals</summary>
    [HttpGet]
    [HttpGet("my")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PaginatedResponse<DealResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _dealService.ListDealsAsync(GetUserId(), pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a deal by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _dealService.GetDealAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Update a draft deal</summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateDealRequest request)
    {
        var response = await _dealService.UpdateDealAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get all available transaction types</summary>
    [HttpGet("~/api/transaction-types")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<DealTypeResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetTypesAsync()
    {
        var response = await _dealService.GetDealTypesAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Invite a counterparty to the deal</summary>
    [HttpPost("{id:guid}/invite")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> InviteAsync(Guid id, [FromBody] InvitePartyRequest request)
    {
        var response = await _orchestrator.InvitePartyAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Accept a deal invitation</summary>
    [HttpPost("{id:guid}/accept")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AcceptAsync(Guid id)
    {
        var response = await _orchestrator.AcceptInvitationAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Reject a deal invitation</summary>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RejectAsync(Guid id)
    {
        var response = await _orchestrator.RejectInvitationAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Propose terms for the deal</summary>
    [HttpPost("{id:guid}/terms")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ProposeTermsAsync(Guid id, [FromBody] ProposeTermsRequest request)
    {
        var response = await _orchestrator.ProposeTermsAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Approve the proposed terms</summary>
    [HttpPost("{id:guid}/approve-terms")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ApproveTermsAsync(Guid id)
    {
        var response = await _orchestrator.ApproveTermsAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Initiate funding for the deal (buyer transfers escrow amount to platform)</summary>
    [HttpPost("{id:guid}/fund")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> FundAsync(Guid id)
    {
        var response = await _orchestrator.InitiateFundingAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit delivery for the deal (seller marks goods/services as delivered)</summary>
    [HttpPost("{id:guid}/deliver")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DeliverAsync(Guid id)
    {
        var response = await _orchestrator.SubmitDeliveryAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Confirm delivery and release escrowed funds to the seller</summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ConfirmAsync(Guid id)
    {
        var response = await _orchestrator.ConfirmDeliveryAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Cancel the deal and refund the buyer if funds were escrowed</summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CancelAsync(Guid id)
    {
        var response = await _orchestrator.CancelDealAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    // ── Delivery card / handover review / funding review ──────────────────
    // Additive: does not replace /deliver or /confirm above.

    /// <summary>Seller generates (or regenerates) the buyer-locked delivery card</summary>
    [HttpPost("{id:guid}/delivery/card")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GenerateDeliveryCardAsync(Guid id)
    {
        var response = await _orchestrator.GenerateDeliveryCardAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Seller invalidates an active, unused delivery card</summary>
    [HttpPost("{id:guid}/delivery/card/invalidate")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> InvalidateDeliveryCardAsync(Guid id)
    {
        var response = await _orchestrator.InvalidateDeliveryCardAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Buyer confirms receipt using the card's token or OTP, starting the handover review window</summary>
    [HttpPost("{id:guid}/delivery/receipt")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ConfirmDeliveryReceiptAsync(Guid id, [FromBody] ConfirmDeliveryReceiptRequest request)
    {
        var response = await _orchestrator.ConfirmDeliveryReceiptAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Buyer confirms the correct product during handover, starting the funding review window</summary>
    [HttpPost("{id:guid}/delivery/handover/complete")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CompleteHandoverReviewAsync(Guid id)
    {
        var response = await _orchestrator.CompleteHandoverReviewAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Buyer approves early release during an active funding review</summary>
    [HttpPost("{id:guid}/delivery/release/approve")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ApproveEarlyReleaseAsync(Guid id)
    {
        var response = await _orchestrator.ApproveEarlyReleaseAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    // ── Deal identity capture ───────────────────────────────────────────

    /// <summary>View a deal identity capture's photo (gated by deal-party authorization and the retention window)</summary>
    [HttpGet("{dealId:guid}/identity-captures/{captureId:guid}/view")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<DealIdentityCaptureResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(403, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ViewIdentityCaptureAsync(Guid dealId, Guid captureId)
    {
        var response = await _dealService.GetIdentityCaptureViewAsync(dealId, captureId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
