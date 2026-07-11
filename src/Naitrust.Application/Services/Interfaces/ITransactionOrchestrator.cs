using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface ITransactionOrchestrator
{
    Task<NaitrustResponse<TransactionResponse>> InvitePartyAsync(Guid transactionId, Guid userId, InvitePartyRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> AcceptInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> RejectInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> ProposeTermsAsync(Guid transactionId, Guid userId, ProposeTermsRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> ApproveTermsAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> InitiateFundingAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> SubmitDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> ConfirmDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> CancelTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
}
