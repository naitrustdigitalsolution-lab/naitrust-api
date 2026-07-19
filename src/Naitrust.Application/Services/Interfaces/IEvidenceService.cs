using Naitrust.Domain.Models.Dtos.Requests.Evidence;
using Naitrust.Domain.Models.Dtos.Responses.Evidence;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IEvidenceService
{
    /// <summary>
    /// Uploads an evidence file (image, document, etc.) associated with a transaction.
    /// </summary>
    Task<NaitrustResponse<EvidenceFileResponse>> UploadEvidenceAsync(Guid userId, UploadEvidenceRequest request, Stream fileStream, string fileName, CancellationToken ct = default);

    /// <summary>
    /// Retrieves evidence file metadata by its unique identifier.
    /// </summary>
    Task<NaitrustResponse<EvidenceFileResponse>> GetEvidenceAsync(Guid evidenceId, CancellationToken ct = default);

    /// <summary>
    /// Lists all evidence files associated with a given transaction.
    /// </summary>
    Task<NaitrustResponse<List<EvidenceFileResponse>>> ListEvidenceByTransactionAsync(Guid transactionId, CancellationToken ct = default);
}
