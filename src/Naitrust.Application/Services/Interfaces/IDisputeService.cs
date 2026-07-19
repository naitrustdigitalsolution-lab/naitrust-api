using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IDisputeService
{
    /// <summary>
    /// Opens a new dispute against a transaction, providing a reason and description.
    /// </summary>
    Task<NaitrustResponse<DisputeResponse>> OpenDisputeAsync(Guid userId, OpenDisputeRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a dispute by its unique identifier, including messages and evidence.
    /// </summary>
    Task<NaitrustResponse<DisputeResponse>> GetDisputeAsync(Guid disputeId, CancellationToken ct = default);

    /// <summary>
    /// Lists all disputes associated with a given transaction.
    /// </summary>
    Task<NaitrustResponse<List<DisputeResponse>>> ListDisputesByTransactionAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Adds a message to an existing dispute conversation thread.
    /// </summary>
    Task<NaitrustResponse<DisputeMessageResponse>> AddMessageAsync(Guid disputeId, Guid userId, AddDisputeMessageRequest request, CancellationToken ct = default);

    /// <summary>
    /// Attaches evidence (file reference) to an existing dispute.
    /// </summary>
    Task<NaitrustResponse<DisputeEvidenceResponse>> AddEvidenceAsync(Guid disputeId, Guid userId, AddDisputeEvidenceRequest request, CancellationToken ct = default);

    /// <summary>
    /// Resolves a dispute with a final decision and optional notes.
    /// </summary>
    Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, Guid userId, ResolveDisputeRequest request, CancellationToken ct = default);
}
