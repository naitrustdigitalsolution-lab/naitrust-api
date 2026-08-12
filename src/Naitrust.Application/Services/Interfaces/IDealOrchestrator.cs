using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

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
}
