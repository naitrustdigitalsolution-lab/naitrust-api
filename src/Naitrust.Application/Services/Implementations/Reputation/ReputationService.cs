using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Reputation;
using Naitrust.Domain.Models.Dtos.Responses.Reputation;

namespace Naitrust.Application.Services.Implementations.Reputation;

public class ReputationService : IReputationService
{
    public Task<NaitrustResponse<ReputationProfileResponse>> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<ReputationProfileResponse>> GetMyProfileAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<ReviewResponse>> SubmitReviewAsync(Guid userId, SubmitReviewRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> UpdateReputationCountersAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
