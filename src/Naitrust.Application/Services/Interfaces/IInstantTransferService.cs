using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.InstantTransfers;
using Naitrust.Domain.Models.Dtos.Responses.InstantTransfers;

namespace Naitrust.Application.Services.Interfaces;

public interface IInstantTransferService
{
    Task<NaitrustResponse<ValidateRecipientResponse>> ValidateRecipientAsync(Guid userId, ValidateRecipientRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<InstantTransferResponse>> CreateAsync(Guid userId, CreateInstantTransferRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<InstantTransferResponse>> GetByIdAsync(Guid userId, Guid transferId, CancellationToken ct = default);
    Task<NaitrustResponse<List<InstantTransferResponse>>> GetMyAsync(Guid userId, CancellationToken ct = default);
}
