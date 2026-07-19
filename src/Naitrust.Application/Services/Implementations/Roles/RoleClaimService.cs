using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Roles;
using Naitrust.Domain.Models.Dtos.Responses.Roles;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Roles;

public class RoleClaimService : IRoleClaimService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<NaitrustRole> _roleManager;

    public RoleClaimService(IUnitOfWork unitOfWork, RoleManager<NaitrustRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task<NaitrustResponse<List<RoleClaimResponse>>> GetClaimsByRoleAsync(string role)
    {
        var roleEntity = await _roleManager.FindByNameAsync(role);

        if (roleEntity is null)
        {
            return NaitrustResponse<List<RoleClaimResponse>>.NotFound($"Role '{role}' not found.");
        }

        var repo = _unitOfWork.GetRepository<NaitrustRoleClaim>();
        var claims = await repo.GetAllDataAsync(rc => rc.RoleId == roleEntity.Id && rc.IsActive);
        var claimList = claims
            .Select(rc => new RoleClaimResponse(rc.Id, role, rc.ClaimType!))
            .ToList();

        return NaitrustResponse<List<RoleClaimResponse>>.Success("Role claims retrieved successfully.", claimList);
    }

    public async Task<NaitrustResponse<bool>> AddClaimAsync(RoleClaimRequest request)
    {
        var roleEntity = await _roleManager.FindByNameAsync(request.Role);

        if (roleEntity is null)
        {
            return NaitrustResponse<bool>.NotFound($"Role '{request.Role}' not found.");
        }

        var repo = _unitOfWork.GetRepository<NaitrustRoleClaim>();
        var existing = await repo.GetSingleByAsync(rc => rc.RoleId == roleEntity.Id && rc.ClaimType == request.ClaimType && rc.IsActive);

        if (existing is not null)
        {
            return NaitrustResponse<bool>.Conflict($"Claim '{request.ClaimType}' already exists for role '{request.Role}'.");
        }

        var claim = new NaitrustRoleClaim
        {
            RoleId = roleEntity.Id,
            ClaimType = request.ClaimType,
            ClaimValue = "true",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await repo.AddAsync(claim);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Created("Claim added successfully.", true);
    }

    public async Task<NaitrustResponse<bool>> UpdateClaimAsync(UpdateRoleClaimRequest request)
    {
        var roleEntity = await _roleManager.FindByNameAsync(request.Role);

        if (roleEntity is null)
        {
            return NaitrustResponse<bool>.NotFound($"Role '{request.Role}' not found.");
        }

        var repo = _unitOfWork.GetRepository<NaitrustRoleClaim>();
        var claim = await repo.GetSingleByAsync(rc => rc.RoleId == roleEntity.Id && rc.ClaimType == request.ClaimType && rc.IsActive);

        if (claim is null)
        {
            return NaitrustResponse<bool>.NotFound($"Claim '{request.ClaimType}' not found for role '{request.Role}'.");
        }

        claim.ClaimType = request.NewClaimType;
        claim.UpdatedAt = DateTime.UtcNow;

        repo.Update(claim);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Claim updated successfully.", true);
    }

    public async Task<NaitrustResponse<bool>> RemoveClaimAsync(RoleClaimRequest request)
    {
        var roleEntity = await _roleManager.FindByNameAsync(request.Role);

        if (roleEntity is null)
        {
            return NaitrustResponse<bool>.NotFound($"Role '{request.Role}' not found.");
        }

        var repo = _unitOfWork.GetRepository<NaitrustRoleClaim>();
        var claim = await repo.GetSingleByAsync(rc => rc.RoleId == roleEntity.Id && rc.ClaimType == request.ClaimType && rc.IsActive);

        if (claim is null)
        {
            return NaitrustResponse<bool>.NotFound($"Claim '{request.ClaimType}' not found for role '{request.Role}'.");
        }

        claim.IsActive = false;
        claim.UpdatedAt = DateTime.UtcNow;

        repo.Update(claim);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Claim removed successfully.", true);
    }
}
