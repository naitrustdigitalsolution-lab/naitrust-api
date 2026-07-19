using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Roles;
using Naitrust.Domain.Models.Dtos.Responses.Roles;

namespace Naitrust.Application.Services.Interfaces;

public interface IRoleService
{
    Task<NaitrustResponse<List<RoleResponse>>> GetAllRolesAsync();
    Task<NaitrustResponse<List<string>>> GetUserRolesAsync(string email);
    Task<NaitrustResponse<RoleResponse>> CreateRoleAsync(CreateRoleRequest request);
    Task<NaitrustResponse<RoleResponse>> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request);
    Task<NaitrustResponse<bool>> DeleteRoleAsync(Guid roleId);
    Task<NaitrustResponse<bool>> AssignRoleAsync(AssignRoleRequest request);
    Task<NaitrustResponse<bool>> RemoveFromRoleAsync(AssignRoleRequest request);
}
