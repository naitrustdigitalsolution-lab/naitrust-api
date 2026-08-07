using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<NaitrustUser> _userManager;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public BusinessService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<BusinessResponse>> CreateBusinessAsync(Guid userId, CreateBusinessRequest request, CancellationToken ct = default)
    {
        var businessRepo = _unitOfWork.GetRepository<Business>();
        var memberRepo = _unitOfWork.GetRepository<BusinessMember>();

        var user = await _userManager.FindByIdAsync(userId.ToString());
        var ownerName = request.OwnerName ?? (user is not null ? $"{user.FirstName} {user.LastName}".Trim() : null);

        var slug = !string.IsNullOrWhiteSpace(request.Slug)
            ? request.Slug
            : GenerateSlug(request.Name);

        var ntId = GenerateNtId();
        var phone = request.PhoneNumber ?? request.Phone;

        var business = new Business
        {
            Id = Guid.NewGuid(),
            OwnerUserId = userId,
            Name = request.Name,
            Slug = slug,
            NtId = ntId,
            Type = request.Type,
            Description = request.Description,
            OwnerName = ownerName,
            Email = request.Email,
            Phone = phone,
            Website = request.Website,
            RegistrationNumber = request.RegistrationNumber,
            TaxId = request.TaxId,
            Country = request.Country,
            State = request.State,
            City = request.City,
            Address = request.Address,
            SocialHandles = request.SocialHandles,
            PaymentAccountBankName = request.PaymentAccountBankName,
            PaymentAccountNumber = request.PaymentAccountNumber,
            PaymentAccountName = request.PaymentAccountName,
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

        if (request.Name is not null) business.Name = request.Name;
        if (request.Slug is not null) business.Slug = request.Slug;
        if (request.Description is not null) business.Description = request.Description;
        if (request.OwnerName is not null) business.OwnerName = request.OwnerName;
        if (request.Email is not null) business.Email = request.Email;
        var phone = request.PhoneNumber ?? request.Phone;
        if (phone is not null) business.Phone = phone;
        if (request.Website is not null) business.Website = request.Website;
        if (request.RegistrationNumber is not null) business.RegistrationNumber = request.RegistrationNumber;
        if (request.TaxId is not null) business.TaxId = request.TaxId;
        if (request.Country is not null) business.Country = request.Country;
        if (request.State is not null) business.State = request.State;
        if (request.City is not null) business.City = request.City;
        if (request.Address is not null) business.Address = request.Address;
        if (request.SocialHandles is not null) business.SocialHandles = request.SocialHandles;
        if (request.PaymentAccountBankName is not null) business.PaymentAccountBankName = request.PaymentAccountBankName;
        if (request.PaymentAccountNumber is not null) business.PaymentAccountNumber = request.PaymentAccountNumber;
        if (request.PaymentAccountName is not null) business.PaymentAccountName = request.PaymentAccountName;

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

    public async Task<NaitrustResponse<List<BusinessResponse>>> SearchAsync(string query, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Business>();
        var q = query.ToLowerInvariant();
        var businesses = await repo.GetAllDataAsync(b =>
            !b.IsDeleted && (
                b.Name.ToLower().Contains(q) ||
                (b.NtId != null && b.NtId.ToLower().Contains(q)) ||
                (b.Slug != null && b.Slug.ToLower().Contains(q))));

        var results = businesses.Select(MapToResponse).ToList();
        return NaitrustResponse<List<BusinessResponse>>.Success("Search results.", results);
    }

    public async Task<NaitrustResponse<BusinessResponse>> GetPublicProfileAsync(string slugOrId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Business>();

        Business? business = null;
        if (Guid.TryParse(slugOrId, out var id))
        {
            business = await repo.GetByIdAsync(id);
        }

        if (business is null)
        {
            business = await repo.GetSingleByAsync(b => b.Slug == slugOrId && !b.IsDeleted);
        }

        if (business is null || business.IsDeleted)
        {
            return NaitrustResponse<BusinessResponse>.NotFound("Business not found.");
        }

        return NaitrustResponse<BusinessResponse>.Success("Business profile retrieved.", MapToResponse(business));
    }

    private static string GenerateSlug(string name)
    {
        var slug = name.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = slug.Trim('-');
        return slug;
    }

    private static string GenerateNtId()
    {
        var random = Random.Shared.Next(10000, 99999);
        return $"NT-BIZ-{random}";
    }

    private static BusinessResponse MapToResponse(Business business)
    {
        var verified = business.VerificationStatus == BusinessVerificationStatus.Verified;

        // Parse SocialHandles JSON string into structured list
        List<SocialHandleDto>? socialHandles = null;
        if (!string.IsNullOrEmpty(business.SocialHandles))
        {
            try
            {
                socialHandles = JsonSerializer.Deserialize<List<SocialHandleDto>>(business.SocialHandles, JsonOptions);
            }
            catch
            {
                // If it's not valid JSON, ignore
            }
        }

        // Wrap flat payment fields into nested object
        PaymentAccountDto? paymentAccount = null;
        if (!string.IsNullOrEmpty(business.PaymentAccountBankName) ||
            !string.IsNullOrEmpty(business.PaymentAccountNumber) ||
            !string.IsNullOrEmpty(business.PaymentAccountName))
        {
            paymentAccount = new PaymentAccountDto(
                business.PaymentAccountBankName ?? "",
                business.PaymentAccountNumber ?? "",
                business.PaymentAccountName ?? "");
        }

        return new BusinessResponse(
            business.Id,
            business.OwnerUserId,
            business.Name,
            business.Slug,
            business.NtId,
            business.Type,
            business.Description,
            business.OwnerName,
            business.Email,
            business.Phone,
            business.Website,
            business.RegistrationNumber,
            business.TaxId,
            business.Country,
            business.State,
            business.City,
            business.Address,
            socialHandles,
            paymentAccount,
            business.VerificationStatus.ToString(),
            verified,
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
