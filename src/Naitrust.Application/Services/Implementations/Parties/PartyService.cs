using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Parties;

public class PartyService : IPartyService
{
    private readonly IUnitOfWork _unitOfWork;

    public PartyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<DealPartyResponse>> CreatePartyAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var dealRepo = _unitOfWork.GetRepository<Deal>();

        var deal = await dealRepo.GetByIdAsync(dealId);
        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealPartyResponse>.NotFound("Deal not found.");
        }

        // Attempt to look up user display name
        var displayName = string.Empty;
        var userRepo = _unitOfWork.GetRepository<NaitrustUser>();
        var user = await userRepo.GetSingleByAsync(u => u.Id == userId);
        if (user is not null)
        {
            displayName = $"{user.FirstName} {user.LastName}".Trim();
        }

        var party = new DealParty
        {
            Id = Guid.NewGuid(),
            DealId = dealId,
            UserId = userId,
            PartyType = PartyType.Buyer,
            PartyMode = deal.PartyMode,
            DisplayName = displayName,
            Email = user?.Email,
            Status = DealPartyStatus.Invited,
            IsActive = true
        };

        await partyRepo.AddAsync(party);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DealPartyResponse>.Created(
            "Party created successfully.",
            MapToPartyResponse(party));
    }

    public async Task<NaitrustResponse<DealPartyResponse>> GetPartyAsync(Guid partyId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var party = await partyRepo.GetByIdAsync(partyId);

        if (party is null || party.IsDeleted)
        {
            return NaitrustResponse<DealPartyResponse>.NotFound("Party not found.");
        }

        return NaitrustResponse<DealPartyResponse>.Success(
            "Party retrieved successfully.",
            MapToPartyResponse(party));
    }

    public async Task<NaitrustResponse<List<DealPartyResponse>>> GetPartiesByDealAsync(Guid dealId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.DealId == dealId && !p.IsDeleted);

        var responses = parties.Select(MapToPartyResponse).ToList();

        return NaitrustResponse<List<DealPartyResponse>>.Success(
            "Parties retrieved successfully.",
            responses);
    }

    public async Task<NaitrustResponse<DealPartyResponse>> ResolvePartyAsync(Guid partyId, Guid userId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var party = await partyRepo.GetByIdAsync(partyId);

        if (party is null || party.IsDeleted)
        {
            return NaitrustResponse<DealPartyResponse>.NotFound("Party not found.");
        }

        party.UserId = userId;
        party.Status = DealPartyStatus.Accepted;
        party.AcceptedAt = DateTime.UtcNow;
        party.UpdatedAt = DateTime.UtcNow;

        // Update display name from user if available
        var userRepo = _unitOfWork.GetRepository<NaitrustUser>();
        var user = await userRepo.GetSingleByAsync(u => u.Id == userId);
        if (user is not null)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            if (!string.IsNullOrEmpty(fullName))
            {
                party.DisplayName = fullName;
            }
        }

        await partyRepo.UpdateAsync(party);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DealPartyResponse>.Success(
            "Party resolved successfully.",
            MapToPartyResponse(party));
    }

    private static DealPartyResponse MapToPartyResponse(DealParty party)
    {
        return new DealPartyResponse(
            party.Id,
            party.UserId,
            party.BusinessId,
            party.PartyType.ToString(),
            party.DisplayName,
            party.Email,
            party.Status.ToString(),
            party.AcceptedAt);
    }
}
