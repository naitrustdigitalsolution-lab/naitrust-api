using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IDisputeService
{
    Task<NaitrustResponse<DisputeResponse>> OpenDisputeAsync(Guid userId, OpenDisputeRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeResponse>> GetDisputeAsync(Guid disputeId, CancellationToken ct = default);
    Task<NaitrustResponse<List<DisputeResponse>>> ListDisputesByTransactionAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeMessageResponse>> AddMessageAsync(Guid disputeId, Guid userId, AddDisputeMessageRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeEvidenceResponse>> AddEvidenceAsync(Guid disputeId, Guid userId, AddDisputeEvidenceRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, Guid userId, ResolveDisputeRequest request, CancellationToken ct = default);
}
