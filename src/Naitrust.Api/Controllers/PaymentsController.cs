using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Create a settlement account for the authenticated user or a business
    /// </summary>
    [HttpPost("settlement-accounts")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateSettlementAccountAsync([FromBody] CreateSettlementAccountRequest request)
    {
        var response = await _paymentService.CreateSettlementAccountAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get the authenticated user's settlement account
    /// </summary>
    [HttpGet("settlement-accounts/me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMySettlementAccountAsync()
    {
        var response = await _paymentService.GetSettlementAccountAsync(GetUserId(), null);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get a business's settlement account
    /// </summary>
    [HttpGet("settlement-accounts/business/{businessId:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetBusinessSettlementAccountAsync(Guid businessId)
    {
        var response = await _paymentService.GetSettlementAccountAsync(GetUserId(), businessId);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get payment status for a transaction
    /// </summary>
    [HttpGet("transactions/{id:guid}/payment-status")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetPaymentStatusAsync(Guid id)
    {
        var response = await _paymentService.GetPaymentStatusAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Request release of escrowed funds
    /// </summary>
    [HttpPost("transactions/{id:guid}/request-release")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RequestReleaseAsync(Guid id, [FromBody] RequestReleaseRequest request)
    {
        var response = await _paymentService.RequestReleaseAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get ledger entries for a transaction
    /// </summary>
    [HttpGet("transactions/{id:guid}/ledger")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetLedgerAsync(Guid id, [FromQuery] PaginationRequest pagination)
    {
        var response = await _paymentService.GetLedgerAsync(id, pagination);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get reconciliation status for a transaction
    /// </summary>
    [HttpGet("transactions/{id:guid}/reconciliation-status")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetReconciliationStatusAsync(Guid id)
    {
        var response = await _paymentService.GetReconciliationStatusAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Validate a payout bank account
    /// </summary>
    [HttpPost("payment-partners/{partner}/validate-payout-account")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ValidatePayoutAccountAsync(string partner, [FromBody] ValidatePayoutAccountRequest request)
    {
        var response = await _paymentService.ValidatePayoutAccountAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
