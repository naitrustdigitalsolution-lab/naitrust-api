using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;
using Naitrust.Domain.Models.Dtos.Responses.Businesses;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Businesses;

public class BusinessService : IBusinessService
{
    private readonly IUnitOfWork _unitOfWork;

    public BusinessService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<BusinessResponse>> CreateBusinessAsync(Guid userId, CreateBusinessRequest request, CancellationToken ct = default)
    {
        var businessRepo = _unitOfWork.GetRepository<Business>();
        var memberRepo = _unitOfWork.GetRepository<BusinessMember>();

        var business = new Business
        {
            Id = Guid.NewGuid(),
            OwnerUserId = userId,
            Name = request.Name,
            Type = request.Type,
            RegistrationNumber = request.RegistrationNumber,
            TaxId = request.TaxId,
            Country = request.Country,
            State = request.State,
            Address = request.Address,
            VerificationStatus = BusinessVerificationStatus.NotStarted,
            IsActive = true
        };

        await businessRepo.AddAsync(business);

        var member = new BusinessMember
        {
            Id = Guid.NewGuid(),
            BusinessId = business.Id,
            UserId = userId,
            Role = BusinessMemberRole.Owner,
            Status = BusinessMemberStatus.Active,
            IsActive = true
        };

        await memberRepo.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<BusinessResponse>.Created("Business created successfully.", MapToResponse(business));
    }

    public async Task<NaitrustResponse<BusinessResponse>> GetBusinessAsync(Guid businessId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Business>();
        var business = await repo.GetByIdAsync(businessId);

        if (business is null || business.IsDeleted)
        {
            return NaitrustResponse<BusinessResponse>.NotFound("Business not found.");
        }

        return NaitrustResponse<BusinessResponse>.Success("Business retrieved successfully.", MapToResponse(business));
    }

    public async Task<NaitrustResponse<List<BusinessResponse>>> GetMyBusinessesAsync(Guid userId, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetRepository<BusinessMember>();
        var businessRepo = _unitOfWork.GetRepository<Business>();

        var memberships = await memberRepo.GetAllDataAsync(m => m.UserId == userId && !m.IsDeleted);

        var businesses = new List<BusinessResponse>();
        foreach (var membership in memberships)
        {
            var business = await businessRepo.GetByIdAsync(membership.BusinessId);
            if (business is not null && !business.IsDeleted)
            {
                businesses.Add(MapToResponse(business));
            }
        }

        return NaitrustResponse<List<BusinessResponse>>.Success("Businesses retrieved successfully.", businesses);
    }

    public async Task<NaitrustResponse<BusinessResponse>> UpdateBusinessAsync(Guid businessId, UpdateBusinessRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Business>();
        var business = await repo.GetByIdAsync(businessId);

        if (business is null || business.IsDeleted)
        {
            return NaitrustResponse<BusinessResponse>.NotFound("Business not found.");
        }

        if (request.Name is not null)
        {
            business.Name = request.Name;
        }

        if (request.Address is not null)
        {
            business.Address = request.Address;
        }

        if (request.State is not null)
        {
            business.State = request.State;
        }

        if (request.TaxId is not null)
        {
            business.TaxId = request.TaxId;
        }

        await repo.UpdateAsync(business);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<BusinessResponse>.Success("Business updated successfully.", MapToResponse(business));
    }

    public async Task<NaitrustResponse<BusinessMemberResponse>> AddMemberAsync(Guid businessId, AddBusinessMemberRequest request, CancellationToken ct = default)
    {
        var businessRepo = _unitOfWork.GetRepository<Business>();
        var business = await businessRepo.GetByIdAsync(businessId);

        if (business is null || business.IsDeleted)
        {
            return NaitrustResponse<BusinessMemberResponse>.NotFound("Business not found.");
        }

        if (!Enum.TryParse<BusinessMemberRole>(request.Role, ignoreCase: true, out var role))
        {
            return NaitrustResponse<BusinessMemberResponse>.BadRequest($"Invalid role: {request.Role}");
        }

        var memberRepo = _unitOfWork.GetRepository<BusinessMember>();

        var member = new BusinessMember
        {
            Id = Guid.NewGuid(),
            BusinessId = businessId,
            UserId = request.UserId,
            Role = role,
            Status = BusinessMemberStatus.Active,
            IsActive = true
        };

        await memberRepo.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<BusinessMemberResponse>.Created("Member added successfully.", MapToMemberResponse(member));
    }

    public async Task<NaitrustResponse<BusinessMemberResponse>> UpdateMemberAsync(Guid businessId, Guid memberId, UpdateBusinessMemberRequest request, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetRepository<BusinessMember>();
        var member = await memberRepo.GetSingleByAsync(m => m.Id == memberId && m.BusinessId == businessId && !m.IsDeleted);

        if (member is null)
        {
            return NaitrustResponse<BusinessMemberResponse>.NotFound("Business member not found.");
        }

        if (request.Role is not null)
        {
            if (!Enum.TryParse<BusinessMemberRole>(request.Role, ignoreCase: true, out var role))
            {
                return NaitrustResponse<BusinessMemberResponse>.BadRequest($"Invalid role: {request.Role}");
            }
            member.Role = role;
        }

        if (request.Status is not null)
        {
            if (!Enum.TryParse<BusinessMemberStatus>(request.Status, ignoreCase: true, out var status))
            {
                return NaitrustResponse<BusinessMemberResponse>.BadRequest($"Invalid status: {request.Status}");
            }
            member.Status = status;
        }

        await memberRepo.UpdateAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<BusinessMemberResponse>.Success("Member updated successfully.", MapToMemberResponse(member));
    }

    private static BusinessResponse MapToResponse(Business business)
    {
        return new BusinessResponse(
            business.Id,
            business.OwnerUserId,
            business.Name,
            business.Type,
            business.RegistrationNumber,
            business.TaxId,
            business.Country,
            business.State,
            business.Address,
            business.VerificationStatus.ToString(),
            business.RiskLevel?.ToString(),
            business.BusinessVerifiedAt,
            business.OwnershipVerifiedAt,
            business.CreatedAt);
    }

    private static BusinessMemberResponse MapToMemberResponse(BusinessMember member)
    {
        return new BusinessMemberResponse(
            member.Id,
            member.UserId,
            member.Role.ToString(),
            member.Status.ToString(),
            member.CreatedAt);
    }
}
