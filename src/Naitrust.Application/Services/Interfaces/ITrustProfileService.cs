using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.TrustProfile;

namespace Naitrust.Application.Services.Interfaces;

public interface ITrustProfileService
{
    Task<NaitrustResponse<TrustProfileResponse>> GetMyAsync(Guid userId, CancellationToken ct = default);
}
