using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Roles;
using Naitrust.Domain.Models.Dtos.Responses.Roles;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Implementations.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<NaitrustRole> _roleManager;
    private readonly UserManager<NaitrustUser> _userManager;

    public RoleService(RoleManager<NaitrustRole> roleManager, UserManager<NaitrustUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public Task<NaitrustResponse<List<RoleResponse>>> GetAllRolesAsync()
    {
        var roles = _roleManager.Roles
            .Select(r => new RoleResponse(r.Id, r.Name!, r.Description, r.CreatedAt))
            .ToList();

        return Task.FromResult(
            NaitrustResponse<List<RoleResponse>>.Success("Roles retrieved successfully.", roles));
    }

    public async Task<NaitrustResponse<List<string>>> GetUserRolesAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return NaitrustResponse<List<string>>.NotFound("User not found.");
        }

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        return NaitrustResponse<List<string>>.Success("User roles retrieved successfully.", roles);
    }

    public async Task<NaitrustResponse<RoleResponse>> CreateRoleAsync(CreateRoleRequest request)
    {
        var existingRole = await _roleManager.FindByNameAsync(request.Name);

        if (existingRole is not null)
        {
            return NaitrustResponse<RoleResponse>.Conflict($"Role '{request.Name}' already exists.");
        }

        var role = new NaitrustRole
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<RoleResponse>.BadRequest($"Failed to create role: {errors}");
        }

        var response = new RoleResponse(role.Id, role.Name!, role.Description, role.CreatedAt);

        return NaitrustResponse<RoleResponse>.Created("Role created successfully.", response);
    }

    public async Task<NaitrustResponse<RoleResponse>> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());

        if (role is null)
        {
            return NaitrustResponse<RoleResponse>.NotFound("Role not found.");
        }

        if (request.Name is not null)
        {
            role.Name = request.Name;
        }

        if (request.Description is not null)
        {
            role.Description = request.Description;
        }

        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<RoleResponse>.BadRequest($"Failed to update role: {errors}");
        }

        var response = new RoleResponse(role.Id, role.Name!, role.Description, role.CreatedAt);

        return NaitrustResponse<RoleResponse>.Success("Role updated successfully.", response);
    }

    public async Task<NaitrustResponse<bool>> DeleteRoleAsync(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());

        if (role is null)
        {
            return NaitrustResponse<bool>.NotFound("Role not found.");
        }

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<bool>.BadRequest($"Failed to delete role: {errors}");
        }

        return NaitrustResponse<bool>.Success("Role deleted successfully.", true);
    }

    public async Task<NaitrustResponse<bool>> AssignRoleAsync(AssignRoleRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return NaitrustResponse<bool>.NotFound("User not found.");
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
        {
            return NaitrustResponse<bool>.NotFound($"Role '{request.Role}' not found.");
        }

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);

        if (isInRole)
        {
            return NaitrustResponse<bool>.Conflict($"User is already in role '{request.Role}'.");
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<bool>.BadRequest($"Failed to assign role: {errors}");
        }

        return NaitrustResponse<bool>.Success("Role assigned successfully.", true);
    }

    public async Task<NaitrustResponse<bool>> RemoveFromRoleAsync(AssignRoleRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return NaitrustResponse<bool>.NotFound("User not found.");
        }

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);

        if (!isInRole)
        {
            return NaitrustResponse<bool>.BadRequest($"User is not in role '{request.Role}'.");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return NaitrustResponse<bool>.BadRequest($"Failed to remove role: {errors}");
        }

        return NaitrustResponse<bool>.Success("Role removed successfully.", true);
    }
}
