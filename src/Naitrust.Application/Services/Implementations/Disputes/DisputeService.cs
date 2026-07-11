using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;

namespace Naitrust.Application.Services.Implementations.Disputes;

public class DisputeService : IDisputeService
{
    public Task<NaitrustResponse<DisputeResponse>> OpenDisputeAsync(Guid userId, OpenDisputeRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<DisputeResponse>> GetDisputeAsync(Guid disputeId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<List<DisputeResponse>>> ListDisputesByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<DisputeMessageResponse>> AddMessageAsync(Guid disputeId, Guid userId, AddDisputeMessageRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<DisputeEvidenceResponse>> AddEvidenceAsync(Guid disputeId, Guid userId, AddDisputeEvidenceRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, Guid userId, ResolveDisputeRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
