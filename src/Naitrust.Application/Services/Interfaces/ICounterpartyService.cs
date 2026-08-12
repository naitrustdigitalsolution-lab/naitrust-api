using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Counterparties;

namespace Naitrust.Application.Services.Interfaces;

public interface ICounterpartyService
{
    Task<NaitrustResponse<List<CounterpartyProfileResponse>>> ListAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<CounterpartyProfileResponse>> ToggleFavouriteAsync(Guid userId, Guid counterpartyUserId, CancellationToken ct = default);
    Task<NaitrustResponse<CounterpartyProfileResponse>> ToggleBlockAsync(Guid userId, Guid counterpartyUserId, CancellationToken ct = default);
}
