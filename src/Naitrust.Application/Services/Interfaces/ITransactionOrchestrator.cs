using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface ITransactionOrchestrator
{
    /// <summary>
    /// Invites a counterparty to the transaction, transitioning it from Draft to PendingCounterparty.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> InvitePartyAsync(Guid transactionId, Guid userId, InvitePartyRequest request, CancellationToken ct = default);

    /// <summary>
    /// Accepts a transaction invitation, transitioning it from PendingCounterparty to TermsNegotiation.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> AcceptInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Rejects a transaction invitation, transitioning it from PendingCounterparty to Cancelled.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> RejectInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Submits proposed terms (agreement details, milestones) for the transaction during negotiation.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> ProposeTermsAsync(Guid transactionId, Guid userId, ProposeTermsRequest request, CancellationToken ct = default);

    /// <summary>
    /// Approves the proposed terms, transitioning the transaction from TermsNegotiation to AwaitingFunding.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> ApproveTermsAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Initiates the funding process for the transaction.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> InitiateFundingAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Marks delivery as submitted by the seller, transitioning from Funded to DeliveryInProgress.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> SubmitDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Confirms delivery receipt by the buyer, transitioning from DeliveryInProgress to Completed.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> ConfirmDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Cancels the transaction from any active status, transitioning it to Cancelled.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> CancelTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
}
