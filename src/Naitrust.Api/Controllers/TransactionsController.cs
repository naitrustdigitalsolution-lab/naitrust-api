using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ITransactionOrchestrator _orchestrator;

    public TransactionsController(ITransactionService transactionService, ITransactionOrchestrator orchestrator)
    {
        _transactionService = transactionService;
        _orchestrator = orchestrator;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Create a new transaction
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTransactionRequest request)
    {
        var response = await _transactionService.CreateTransactionAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// List the current user's transactions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _transactionService.ListTransactionsAsync(GetUserId(), pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get a transaction by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _transactionService.GetTransactionAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Update a draft transaction
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateTransactionRequest request)
    {
        var response = await _transactionService.UpdateTransactionAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get all available transaction types
    /// </summary>
    [HttpGet("~/api/transaction-types")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetTypesAsync()
    {
        var response = await _transactionService.GetTransactionTypesAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Invite a counterparty to the transaction
    /// </summary>
    [HttpPost("{id:guid}/invite")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> InviteAsync(Guid id, [FromBody] InvitePartyRequest request)
    {
        var response = await _orchestrator.InvitePartyAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Accept a transaction invitation
    /// </summary>
    [HttpPost("{id:guid}/accept")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AcceptAsync(Guid id)
    {
        var response = await _orchestrator.AcceptInvitationAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Reject a transaction invitation
    /// </summary>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RejectAsync(Guid id)
    {
        var response = await _orchestrator.RejectInvitationAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Propose terms for the transaction
    /// </summary>
    [HttpPost("{id:guid}/terms")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ProposeTermsAsync(Guid id, [FromBody] ProposeTermsRequest request)
    {
        var response = await _orchestrator.ProposeTermsAsync(id, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Approve the proposed terms
    /// </summary>
    [HttpPost("{id:guid}/approve-terms")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ApproveTermsAsync(Guid id)
    {
        var response = await _orchestrator.ApproveTermsAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Initiate funding for the transaction
    /// </summary>
    [HttpPost("{id:guid}/fund")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> FundAsync(Guid id)
    {
        var response = await _orchestrator.InitiateFundingAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Submit delivery for the transaction
    /// </summary>
    [HttpPost("{id:guid}/deliver")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DeliverAsync(Guid id)
    {
        var response = await _orchestrator.SubmitDeliveryAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Confirm delivery and complete the transaction
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ConfirmAsync(Guid id)
    {
        var response = await _orchestrator.ConfirmDeliveryAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Cancel the transaction
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CancelAsync(Guid id)
    {
        var response = await _orchestrator.CancelTransactionAsync(id, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
