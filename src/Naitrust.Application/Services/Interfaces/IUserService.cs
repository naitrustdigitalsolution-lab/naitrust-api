using Naitrust.Domain.Models.Dtos.Requests.Users;
using Naitrust.Domain.Models.Dtos.Responses.Users;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IUserService
{
    Task<NaitrustResponse<UserResponse>> GetUserAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<UserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default);
}
