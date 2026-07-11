using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Evidence;
using Naitrust.Domain.Models.Dtos.Responses.Evidence;

namespace Naitrust.Application.Services.Implementations.Evidence;

public class EvidenceService : IEvidenceService
{
    public Task<NaitrustResponse<EvidenceFileResponse>> UploadEvidenceAsync(Guid userId, UploadEvidenceRequest request, Stream fileStream, string fileName, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<EvidenceFileResponse>> GetEvidenceAsync(Guid evidenceId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<List<EvidenceFileResponse>>> ListEvidenceByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
