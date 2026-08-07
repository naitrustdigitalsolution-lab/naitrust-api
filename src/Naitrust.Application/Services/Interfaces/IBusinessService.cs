using Naitrust.Domain.Models.Dtos.Requests.Businesses;
using Naitrust.Domain.Models.Dtos.Responses.Businesses;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IBusinessService
{
    Task<NaitrustResponse<BusinessResponse>> CreateBusinessAsync(Guid userId, CreateBusinessRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<BusinessResponse>> GetBusinessAsync(Guid businessId, CancellationToken ct = default);
    Task<NaitrustResponse<List<BusinessResponse>>> GetMyBusinessesAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<BusinessResponse>> UpdateBusinessAsync(Guid businessId, UpdateBusinessRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<BusinessMemberResponse>> AddMemberAsync(Guid businessId, AddBusinessMemberRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<BusinessMemberResponse>> UpdateMemberAsync(Guid businessId, Guid memberId, UpdateBusinessMemberRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<List<BusinessResponse>>> SearchAsync(string query, CancellationToken ct = default);
    Task<NaitrustResponse<BusinessResponse>> GetPublicProfileAsync(string slugOrId, CancellationToken ct = default);
}
