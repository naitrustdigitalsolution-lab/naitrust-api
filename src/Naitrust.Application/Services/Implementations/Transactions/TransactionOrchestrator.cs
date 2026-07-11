using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Application.Services.Implementations.Transactions;

// The single authority for all transaction state transitions
public class TransactionOrchestrator : ITransactionOrchestrator
{
    public Task<NaitrustResponse<TransactionResponse>> InvitePartyAsync(Guid transactionId, Guid userId, InvitePartyRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> AcceptInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> RejectInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> ProposeTermsAsync(Guid transactionId, Guid userId, ProposeTermsRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> ApproveTermsAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> InitiateFundingAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> SubmitDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> ConfirmDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> CancelTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
