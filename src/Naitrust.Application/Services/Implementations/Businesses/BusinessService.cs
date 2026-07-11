using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;
using Naitrust.Domain.Models.Dtos.Responses.Businesses;

namespace Naitrust.Application.Services.Implementations.Businesses;

public class BusinessService : IBusinessService
{
    public Task<NaitrustResponse<BusinessResponse>> CreateBusinessAsync(Guid userId, CreateBusinessRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<BusinessResponse>> GetBusinessAsync(Guid businessId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<List<BusinessResponse>>> GetMyBusinessesAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<BusinessResponse>> UpdateBusinessAsync(Guid businessId, UpdateBusinessRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<BusinessMemberResponse>> AddMemberAsync(Guid businessId, AddBusinessMemberRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<BusinessMemberResponse>> UpdateMemberAsync(Guid businessId, Guid memberId, UpdateBusinessMemberRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
