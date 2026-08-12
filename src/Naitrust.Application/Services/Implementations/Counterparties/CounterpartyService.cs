using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Counterparties;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Counterparties;

public class CounterpartyService : ICounterpartyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public CounterpartyService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<List<CounterpartyProfileResponse>>> ListAsync(
        Guid userId, CancellationToken ct = default)
    {
        // Find all deals the current user was a party to
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var myParties = await partyRepo.GetAllDataAsync(p => p.UserId == userId && !p.IsDeleted);
        var myDealIds = myParties.Select(p => p.DealId).Distinct().ToList();

        // Find the other parties in those deals
        var otherParties = await partyRepo.GetAllDataAsync(
            p => myDealIds.Contains(p.DealId) && p.UserId != userId && p.UserId.HasValue && !p.IsDeleted);

        // Group by counterparty userId to compute deal counts
        var grouped = otherParties
            .Where(p => p.UserId.HasValue)
            .GroupBy(p => p.UserId!.Value)
            .ToList();

        // Load preferences
        var prefRepo = _unitOfWork.GetRepository<UserCounterpartyPreference>();
        var prefs = await prefRepo.GetAllDataAsync(
            p => p.OwnerUserId == userId && !p.IsDeleted);
        var prefMap = prefs.ToDictionary(p => p.CounterpartyUserId);

        // Load deal statuses for completed count
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deals = await dealRepo.GetAllDataAsync(d => myDealIds.Contains(d.Id) && !d.IsDeleted);
        var dealStatusMap = deals.ToDictionary(d => d.Id, d => d.Status);

        // Load reputation profiles for ratings
        var repRepo = _unitOfWork.GetRepository<ReputationProfile>();

        var result = new List<CounterpartyProfileResponse>();

        foreach (var group in grouped)
        {
            var counterpartyUserId = group.Key;
            var user = await _userManager.FindByIdAsync(counterpartyUserId.ToString());
            if (user is null || user.IsDeleted) { continue; }

            var dealIds = group.Select(p => p.DealId).ToList();
            var completedCount = dealIds.Count(id =>
                dealStatusMap.TryGetValue(id, out var s) && (s == DealStatus.Completed || s == DealStatus.PaidOut));

            prefMap.TryGetValue(counterpartyUserId, out var pref);

            var rep = await repRepo.GetSingleByAsync(
                r => r.SubjectId == counterpartyUserId && !r.IsDeleted);

            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            var initials = GetInitials(fullName);

            result.Add(new CounterpartyProfileResponse(
                counterpartyUserId,
                NaitrustId: null,
                Name: fullName,
                BusinessName: null,
                Email: user.Email,
                Phone: user.PhoneNumber,
                Address: user.Address,
                Notes: null,
                AvatarInitials: initials,
                Relation: "other",
                IdentityVerified: user.IdentityVerifiedAt.HasValue,
                BusinessVerified: false,
                MemberSince: user.CreatedAt.ToString("O"),
                CompletedDealsCount: completedCount,
                HasPriorTransactionWithYou: dealIds.Count > 0,
                AverageResponseTimeHours: null,
                ResolvedDisputesCount: 0,
                RatingAverage: rep is not null && rep.RatingCount > 0 ? (double)rep.RatingAverage : null,
                IsFavourite: pref?.IsFavourite ?? false,
                IsBlocked: pref?.IsBlocked ?? false));
        }

        return NaitrustResponse<List<CounterpartyProfileResponse>>.Success("Counterparties retrieved.", result);
    }

    public async Task<NaitrustResponse<CounterpartyProfileResponse>> ToggleFavouriteAsync(
        Guid userId, Guid counterpartyUserId, CancellationToken ct = default)
    {
        var pref = await GetOrCreatePrefAsync(userId, counterpartyUserId);
        pref.IsFavourite = !pref.IsFavourite;

        var prefRepo = _unitOfWork.GetRepository<UserCounterpartyPreference>();
        await prefRepo.UpdateAsync(pref);
        await _unitOfWork.SaveChangesAsync();

        return await BuildSingleResponseAsync(userId, counterpartyUserId, pref);
    }

    public async Task<NaitrustResponse<CounterpartyProfileResponse>> ToggleBlockAsync(
        Guid userId, Guid counterpartyUserId, CancellationToken ct = default)
    {
        var pref = await GetOrCreatePrefAsync(userId, counterpartyUserId);
        pref.IsBlocked = !pref.IsBlocked;

        var prefRepo = _unitOfWork.GetRepository<UserCounterpartyPreference>();
        await prefRepo.UpdateAsync(pref);
        await _unitOfWork.SaveChangesAsync();

        return await BuildSingleResponseAsync(userId, counterpartyUserId, pref);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<UserCounterpartyPreference> GetOrCreatePrefAsync(Guid ownerId, Guid counterpartyId)
    {
        var prefRepo = _unitOfWork.GetRepository<UserCounterpartyPreference>();
        var pref = await prefRepo.GetSingleByAsync(
            p => p.OwnerUserId == ownerId && p.CounterpartyUserId == counterpartyId && !p.IsDeleted);

        if (pref is null)
        {
            pref = new UserCounterpartyPreference
            {
                Id = Guid.NewGuid(),
                OwnerUserId = ownerId,
                CounterpartyUserId = counterpartyId,
                IsFavourite = false,
                IsBlocked = false,
                IsActive = true,
            };
            await prefRepo.AddAsync(pref);
        }

        return pref;
    }

    private async Task<NaitrustResponse<CounterpartyProfileResponse>> BuildSingleResponseAsync(
        Guid userId, Guid counterpartyUserId, UserCounterpartyPreference pref)
    {
        var user = await _userManager.FindByIdAsync(counterpartyUserId.ToString());
        if (user is null)
            return NaitrustResponse<CounterpartyProfileResponse>.NotFound("Counterparty not found.");

        var fullName = $"{user.FirstName} {user.LastName}".Trim();

        var response = new CounterpartyProfileResponse(
            counterpartyUserId,
            NaitrustId: null,
            Name: fullName,
            BusinessName: null,
            Email: user.Email,
            Phone: user.PhoneNumber,
            Address: user.Address,
            Notes: null,
            AvatarInitials: GetInitials(fullName),
            Relation: "other",
            IdentityVerified: user.IdentityVerifiedAt.HasValue,
            BusinessVerified: false,
            MemberSince: user.CreatedAt.ToString("O"),
            CompletedDealsCount: 0,
            HasPriorTransactionWithYou: true,
            AverageResponseTimeHours: null,
            ResolvedDisputesCount: 0,
            RatingAverage: null,
            IsFavourite: pref.IsFavourite,
            IsBlocked: pref.IsBlocked);

        return NaitrustResponse<CounterpartyProfileResponse>.Success("Updated.", response);
    }

    private static string GetInitials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[1][0]}".ToUpper()
            : name.Length > 0 ? name[0].ToString().ToUpper() : "?";
    }
}
