using Naitrust.Domain.Models.Dtos.Requests.Reputation;
using Naitrust.Domain.Models.Dtos.Responses.Reputation;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IReputationService
{
    /// <summary>
    /// Retrieves the public reputation profile for a user by their ID.
    /// </summary>
    Task<NaitrustResponse<ReputationProfileResponse>> GetProfileAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the authenticated user's own reputation profile.
    /// </summary>
    Task<NaitrustResponse<ReputationProfileResponse>> GetMyProfileAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Submits a review and rating for a counterparty after a completed transaction.
    /// </summary>
    Task<NaitrustResponse<ReviewResponse>> SubmitReviewAsync(Guid userId, SubmitReviewRequest request, CancellationToken ct = default);

    /// <summary>
    /// Recalculates and updates a user's aggregate reputation counters (avg rating, total reviews, etc.).
    /// </summary>
    Task<NaitrustResponse<bool>> UpdateReputationCountersAsync(Guid userId, CancellationToken ct = default);
}
