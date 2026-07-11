using Naitrust.Domain.Models.Dtos.Requests.Evidence;
using Naitrust.Domain.Models.Dtos.Responses.Evidence;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IEvidenceService
{
    Task<NaitrustResponse<EvidenceFileResponse>> UploadEvidenceAsync(Guid userId, UploadEvidenceRequest request, Stream fileStream, string fileName, CancellationToken ct = default);
    Task<NaitrustResponse<EvidenceFileResponse>> GetEvidenceAsync(Guid evidenceId, CancellationToken ct = default);
    Task<NaitrustResponse<List<EvidenceFileResponse>>> ListEvidenceByTransactionAsync(Guid transactionId, CancellationToken ct = default);
}
