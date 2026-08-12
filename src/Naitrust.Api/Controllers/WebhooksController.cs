using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.ExternalServices;
using Naitrust.Application.ExternalServices.Providus;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Api.Controllers;

/// <summary>
/// Payment partner webhook endpoints (no auth — verified via signatures)
/// </summary>
[ApiController]
[Route("api/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly IPaymentPartnerFactory _partnerFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WebhooksController> _logger;

    public WebhooksController(
        IPaymentPartnerFactory partnerFactory,
        IUnitOfWork unitOfWork,
        ILogger<WebhooksController> logger)
    {
        _partnerFactory = partnerFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // ── Anchor: inbound funding events ──────────────────────────────────────

    /// <summary>
    /// Receive and process payment funding webhooks from a payment partner.
    /// Verifies the signature, then marks the deal's escrow account as funded.
    /// </summary>
    [HttpPost("payment-partners/{partner}/funding")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> PaymentFundingAsync(string partner, CancellationToken ct)
    {
        var (payload, signature) = await ReadWebhookAsync(ct);

        if (!Enum.TryParse<PaymentPartnerId>(partner, ignoreCase: true, out var partnerId))
        {
            _logger.LogWarning("Unknown payment partner in webhook route: {Partner}", partner);
            return Ok(); // always 200 to prevent retries for unknown partners
        }

        VerifiedWebhookEvent webhookEvent;
        try
        {
            var partnerService = _partnerFactory.GetPartner(partnerId);
            webhookEvent = await partnerService.VerifyWebhookAsync(new VerifyWebhookRequest(payload, signature), ct);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Webhook signature verification failed for {Partner}", partner);
            return BadRequest(NaitrustResponse.BadRequest("Invalid webhook signature."));
        }

        // Only process confirmed inbound funding events
        if (webhookEvent.EventType is not ("nip.inbound.completed" or "virtualAccount.credit"))
        {
            _logger.LogInformation("Anchor funding webhook ignored: {Event}", webhookEvent.EventType);
            return Ok(NaitrustResponse.Success(message: "Event acknowledged."));
        }

        await ProcessFundingEventAsync(webhookEvent, ct);
        return Ok(NaitrustResponse.Success(message: "Funding event processed."));
    }

    // ── Anchor: outbound transfer status events ──────────────────────────────

    /// <summary>
    /// Receive and process outbound transfer result webhooks from a payment partner.
    /// Updates the PaymentInstruction status based on transfer outcome.
    /// </summary>
    [HttpPost("payment-partners/{partner}/transfer")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> PaymentTransferAsync(string partner, CancellationToken ct)
    {
        var (payload, signature) = await ReadWebhookAsync(ct);

        if (!Enum.TryParse<PaymentPartnerId>(partner, ignoreCase: true, out var partnerId))
        {
            _logger.LogWarning("Unknown payment partner in webhook route: {Partner}", partner);
            return Ok();
        }

        VerifiedWebhookEvent webhookEvent;
        try
        {
            var partnerService = _partnerFactory.GetPartner(partnerId);
            webhookEvent = await partnerService.VerifyWebhookAsync(new VerifyWebhookRequest(payload, signature), ct);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Webhook signature verification failed for {Partner}", partner);
            return BadRequest(NaitrustResponse.BadRequest("Invalid webhook signature."));
        }

        await ProcessTransferEventAsync(webhookEvent, ct);
        return Ok(NaitrustResponse.Success(message: "Transfer event processed."));
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private async Task<(string payload, string signature)> ReadWebhookAsync(CancellationToken ct)
    {
        Request.EnableBuffering();
        using var reader = new StreamReader(Request.Body, leaveOpen: true);
        var payload = await reader.ReadToEndAsync(ct);
        Request.Body.Position = 0;

        var signature = Request.Headers["x-anchor-signature"].FirstOrDefault() ?? "";
        return (payload, signature);
    }

    private async Task ProcessFundingEventAsync(VerifiedWebhookEvent evt, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(evt.VirtualAccountReference)) return;

        var repo = _unitOfWork.GetRepository<VirtualAccount>();
        var account = await repo.GetSingleByAsync(
            va => va.ProviderReference == evt.VirtualAccountReference);

        if (account is null)
        {
            _logger.LogWarning(
                "Funding webhook for unknown sub-account: {Ref}", evt.VirtualAccountReference);
            return;
        }

        account.AmountReceivedMinor += evt.AmountMinor;
        account.Status = VirtualAccountStatus.Funded;
        account.FundedAt ??= DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(
            "VirtualAccount {Id} marked as funded. Amount: {Amount} kobo.",
            account.Id, evt.AmountMinor);
    }

    private async Task ProcessTransferEventAsync(VerifiedWebhookEvent evt, CancellationToken ct)
    {
        // Map Anchor transfer event types to PaymentInstruction status
        var status = evt.EventType switch
        {
            "nip.outbound.completed" or "transfer.successful" => PaymentInstructionStatus.Confirmed,
            "nip.outbound.failed" or "transfer.failed" => PaymentInstructionStatus.Failed,
            _ => (PaymentInstructionStatus?)null
        };

        if (status is null)
        {
            _logger.LogInformation("Transfer webhook event ignored: {Event}", evt.EventType);
            return;
        }

        var repo = _unitOfWork.GetRepository<PaymentInstruction>();
        var instruction = await repo.GetSingleByAsync(
            pi => pi.PartnerReference == evt.EventId);

        if (instruction is null)
        {
            _logger.LogWarning(
                "Transfer webhook for unknown PartnerReference: {Ref}", evt.EventId);
            return;
        }

        instruction.Status = status.Value;
        instruction.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(
            "PaymentInstruction {Id} updated to {Status}.", instruction.Id, status.Value);
    }
}
