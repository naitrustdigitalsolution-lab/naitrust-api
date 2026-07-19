using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Roles;
using Naitrust.Domain.Models.Dtos.Responses.Roles;

namespace Naitrust.Application.Services.Interfaces;

public interface IRoleClaimService
{
    Task<NaitrustResponse<List<RoleClaimResponse>>> GetClaimsByRoleAsync(string role);
    Task<NaitrustResponse<bool>> AddClaimAsync(RoleClaimRequest request);
    Task<NaitrustResponse<bool>> UpdateClaimAsync(UpdateRoleClaimRequest request);
    Task<NaitrustResponse<bool>> RemoveClaimAsync(RoleClaimRequest request);
}
