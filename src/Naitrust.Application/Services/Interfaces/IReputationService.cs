using Naitrust.Domain.Models.Dtos.Requests.Reputation;
using Naitrust.Domain.Models.Dtos.Responses.Reputation;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IReputationService
{
    Task<NaitrustResponse<ReputationProfileResponse>> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<ReputationProfileResponse>> GetMyProfileAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<ReviewResponse>> SubmitReviewAsync(Guid userId, SubmitReviewRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> UpdateReputationCountersAsync(Guid userId, CancellationToken ct = default);
}
