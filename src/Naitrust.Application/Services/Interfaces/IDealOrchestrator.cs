using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Interfaces;

public interface IDealOrchestrator
{
    /// <summary>
    /// Invites a counterparty to the deal, transitioning it from Draft to PendingCounterparty.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> InvitePartyAsync(Guid dealId, Guid userId, InvitePartyRequest request, CancellationToken ct = default);

    /// <summary>
    /// Accepts a deal invitation, transitioning it from PendingCounterparty to TermsNegotiation.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> AcceptInvitationAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Rejects a deal invitation, transitioning it from PendingCounterparty to Cancelled.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> RejectInvitationAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Submits proposed terms (agreement details, milestones) for the deal during negotiation.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> ProposeTermsAsync(Guid dealId, Guid userId, ProposeTermsRequest request, CancellationToken ct = default);

    /// <summary>
    /// Approves the proposed terms, transitioning the deal from TermsNegotiation to AwaitingFunding.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> ApproveTermsAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Initiates the funding process for the deal.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> InitiateFundingAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Marks delivery as submitted by the seller, transitioning from Funded to DeliveryInProgress.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> SubmitDeliveryAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Confirms delivery receipt by the buyer, transitioning from DeliveryInProgress to Completed.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> ConfirmDeliveryAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Cancels the deal from any active status, transitioning it to Cancelled.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> CancelDealAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    // ── Delivery card / handover review / funding review ──────────────────
    // Additive: does not replace SubmitDeliveryAsync/ConfirmDeliveryAsync above.

    /// <summary>Seller generates (or regenerates) the buyer-locked delivery card.</summary>
    Task<NaitrustResponse<DealResponse>> GenerateDeliveryCardAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>Seller invalidates an active, unused delivery card.</summary>
    Task<NaitrustResponse<DealResponse>> InvalidateDeliveryCardAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>Buyer confirms receipt using the card's token or OTP, starting the handover review window.</summary>
    Task<NaitrustResponse<DealResponse>> ConfirmDeliveryReceiptAsync(Guid dealId, Guid userId, ConfirmDeliveryReceiptRequest request, CancellationToken ct = default);

    /// <summary>Buyer confirms the correct product during handover, starting the funding review window.</summary>
    Task<NaitrustResponse<DealResponse>> CompleteHandoverReviewAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>Buyer approves early release during an active funding review, triggering the real fund release.</summary>
    Task<NaitrustResponse<DealResponse>> ApproveEarlyReleaseAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>Called by DisputeService when a dispute opens with evidence: freezes handover/funding-review release.</summary>
    Task BlockDeliveryReleaseAsync(Guid dealId, CancellationToken ct = default);

    /// <summary>
    /// Read-through used by DealService.GetDealAsync: lazily applies any timer-elapsed transition
    /// (mutating and persisting deal/delivery state as needed) and returns the current lifecycle DTO.
    /// Always returns a non-null lifecycle (an empty/not-started shape when no delivery activity exists yet).
    /// </summary>
    Task<DealDeliveryLifecycleDto> ReconcileAndGetDeliveryStateAsync(Deal deal, CancellationToken ct = default);
}
