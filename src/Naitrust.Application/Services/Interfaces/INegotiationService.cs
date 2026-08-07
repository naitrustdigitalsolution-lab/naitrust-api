using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Negotiations;
using Naitrust.Domain.Models.Dtos.Responses.Negotiations;

namespace Naitrust.Application.Services.Interfaces;

public interface INegotiationService
{
    Task<NaitrustResponse<NegotiationResponse?>> GetByTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<NegotiationResponse>> ProposeAsync(Guid transactionId, Guid userId, ProposeNegotiationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<NegotiationResponse>> RespondToProposalAsync(Guid transactionId, Guid proposalId, Guid userId, RespondToProposalRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<NegotiationResponse?>> WithdrawAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
}
