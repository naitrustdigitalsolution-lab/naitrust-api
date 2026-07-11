using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Users;
using Naitrust.Domain.Models.Dtos.Responses.Users;

namespace Naitrust.Application.Services.Implementations.Users;

public class UserService : IUserService
{
    public Task<NaitrustResponse<UserResponse>> GetUserAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<UserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
