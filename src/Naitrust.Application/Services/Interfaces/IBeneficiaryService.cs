using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Beneficiaries;
using Naitrust.Domain.Models.Dtos.Responses.Beneficiaries;

namespace Naitrust.Application.Services.Interfaces;

public interface IBeneficiaryService
{
    Task<NaitrustResponse<List<BeneficiaryResponse>>> ListAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<BeneficiaryResponse>> CreateAsync(Guid userId, CreateBeneficiaryRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> DeleteAsync(Guid userId, Guid beneficiaryId, CancellationToken ct = default);
}
