using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.TrustProfile;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Disputes;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Domain.Models.Enums.Verification;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.TrustProfile;

public class TrustProfileService : ITrustProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public TrustProfileService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<TrustProfileResponse>> GetMyAsync(
        Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.IsDeleted)
            return NaitrustResponse<TrustProfileResponse>.NotFound("User not found.");

        // Identity verification level
        var identityLevel = user.IdentityVerifiedAt.HasValue ? "full"
            : user.EmailVerifiedAt.HasValue ? "basic"
            : "unverified";

        // Business verification level
        string businessLevel = "none";
        var bizRepo = _unitOfWork.GetRepository<Business>();
        var businesses = await bizRepo.GetAllDataAsync(b => b.OwnerUserId == userId && !b.IsDeleted);
        if (businesses.Any(b => b.VerificationStatus == BusinessVerificationStatus.Verified))
            businessLevel = "verified";
        else if (businesses.Any(b => b.VerificationStatus == BusinessVerificationStatus.Pending))
            businessLevel = "pending";

        // Deal counts — find deals where user is a party
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var myParties = await partyRepo.GetAllDataAsync(p => p.UserId == userId && !p.IsDeleted);
        var myDealIds = myParties.Select(p => p.DealId).Distinct().ToList();

        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var myDeals = await dealRepo.GetAllDataAsync(d => myDealIds.Contains(d.Id) && !d.IsDeleted);

        var completedDeals = myDeals
            .Where(d => d.Status == DealStatus.Completed || d.Status == DealStatus.PaidOut)
            .ToList();
        var cancelledCount = myDeals.Count(d => d.Status == DealStatus.Cancelled);
        var activeCount = myDeals.Count(d =>
            d.Status != DealStatus.Completed &&
            d.Status != DealStatus.PaidOut &&
            d.Status != DealStatus.Cancelled &&
            d.Status != DealStatus.Refunded);

        // Average completion days
        double? avgCompletionDays = null;
        var completedWithDate = completedDeals
            .Where(d => d.UpdatedAt > d.CreatedAt)
            .ToList();
        if (completedWithDate.Count > 0)
            avgCompletionDays = completedWithDate.Average(d => (d.UpdatedAt - d.CreatedAt).TotalDays);

        // Repeat counterparties — other parties appearing in 2+ deals
        var otherParties = await partyRepo.GetAllDataAsync(
            p => myDealIds.Contains(p.DealId) && p.UserId != userId && p.UserId.HasValue && !p.IsDeleted);
        var repeatCount = otherParties
            .Where(p => p.UserId.HasValue)
            .GroupBy(p => p.UserId!.Value)
            .Count(g => g.Count() >= 2);

        // Disputes resolved
        var disputeRepo = _unitOfWork.GetRepository<Dispute>();
        var myDisputes = await disputeRepo.GetAllDataAsync(
            d => myDealIds.Contains(d.DealId) && !d.IsDeleted);
        var resolvedDisputeCount = myDisputes.Count(d =>
            d.Status == DisputeStatus.ResolvedRelease ||
            d.Status == DisputeStatus.ResolvedRefund ||
            d.Status == DisputeStatus.ResolvedSplit);

        // Reputation
        var repRepo = _unitOfWork.GetRepository<ReputationProfile>();
        var rep = await repRepo.GetSingleByAsync(r => r.SubjectId == userId && !r.IsDeleted);

        var profile = new TrustProfileResponse(
            IdentityVerificationLevel: identityLevel,
            BusinessVerificationLevel: businessLevel,
            CompletedDealsCount: completedDeals.Count,
            CancelledDealsCount: cancelledCount,
            ActiveDealsCount: activeCount,
            ResolvedDisputesCount: resolvedDisputeCount,
            RepeatCounterpartiesCount: repeatCount,
            MemberSince: user.CreatedAt.ToString("O"),
            AverageCompletionDays: avgCompletionDays,
            AverageResponseTimeHours: null,
            RatingAverage: rep is not null && rep.RatingCount > 0 ? (double)rep.RatingAverage : null,
            RatingCount: rep?.RatingCount ?? 0);

        return NaitrustResponse<TrustProfileResponse>.Success("Trust profile retrieved.", profile);
    }
}
