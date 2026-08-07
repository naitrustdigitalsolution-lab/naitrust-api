using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IDisputeService
{
    Task<NaitrustResponse<DisputeResponse?>> GetByTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeResponse>> OpenDisputeAsync(Guid transactionId, Guid userId, OpenDisputeRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeResponse>> AddMessageToTransactionDisputeAsync(Guid transactionId, Guid userId, AddDisputeMessageRequest request, CancellationToken ct = default);
}
