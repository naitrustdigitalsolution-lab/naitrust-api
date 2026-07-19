using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Users;
using Naitrust.Domain.Models.Dtos.Responses.Users;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Implementations.Users;

public class UserService : IUserService
{
    private readonly UserManager<NaitrustUser> _userManager;

    public UserService(UserManager<NaitrustUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<UserResponse>> GetUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<UserResponse>.NotFound("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return NaitrustResponse<UserResponse>.Success("User retrieved successfully.", new UserResponse(
            user.Id,
            user.Email!,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            roles,
            user.Status.ToString(),
            user.EmailVerifiedAt,
            user.PhoneVerifiedAt,
            user.IdentityVerifiedAt,
            user.LastLivenessVerifiedAt,
            user.LastTransactionActivityAt,
            user.CreatedAt));
    }

    public async Task<NaitrustResponse<UserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
        {
            return NaitrustResponse<UserResponse>.NotFound("User not found.");
        }

        if (request.FirstName is not null)
        {
            user.FirstName = request.FirstName;
        }

        if (request.LastName is not null)
        {
            user.LastName = request.LastName;
        }

        if (request.Phone is not null)
        {
            user.PhoneNumber = request.Phone;
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<UserResponse>.BadRequest($"Update failed: {errors}");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return NaitrustResponse<UserResponse>.Success("User updated successfully.", new UserResponse(
            user.Id,
            user.Email!,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            roles,
            user.Status.ToString(),
            user.EmailVerifiedAt,
            user.PhoneVerifiedAt,
            user.IdentityVerifiedAt,
            user.LastLivenessVerifiedAt,
            user.LastTransactionActivityAt,
            user.CreatedAt));
    }
}
