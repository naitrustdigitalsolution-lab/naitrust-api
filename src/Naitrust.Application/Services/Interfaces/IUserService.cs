using Naitrust.Domain.Models.Dtos.Requests.Users;
using Naitrust.Domain.Models.Dtos.Responses.Users;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Retrieves a user's profile by their unique identifier.
    /// </summary>
    Task<NaitrustResponse<UserResponse>> GetUserAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Updates the authenticated user's profile fields (name, phone, etc.).
    /// </summary>
    Task<NaitrustResponse<UserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default);
}
