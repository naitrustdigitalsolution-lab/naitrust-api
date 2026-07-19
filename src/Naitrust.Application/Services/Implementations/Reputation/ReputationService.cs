using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Reputation;
using Naitrust.Domain.Models.Dtos.Responses.Reputation;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Verification;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Reputation;

public class ReputationService : IReputationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReputationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<ReputationProfileResponse>> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await GetOrCreateProfileAsync(userId);
        return NaitrustResponse<ReputationProfileResponse>.Success(
            "Reputation profile retrieved successfully.",
            MapToResponse(profile));
    }

    public async Task<NaitrustResponse<ReputationProfileResponse>> GetMyProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await GetOrCreateProfileAsync(userId);
        return NaitrustResponse<ReputationProfileResponse>.Success(
            "Reputation profile retrieved successfully.",
            MapToResponse(profile));
    }

    public async Task<NaitrustResponse<ReviewResponse>> SubmitReviewAsync(Guid userId, SubmitReviewRequest request, CancellationToken ct = default)
    {
        if (request.Rating < 1 || request.Rating > 5)
        {
            return NaitrustResponse<ReviewResponse>.BadRequest("Rating must be between 1 and 5.");
        }

        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(request.TransactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<ReviewResponse>.NotFound("Transaction not found.");
        }

        var reviewRepo = _unitOfWork.GetRepository<Review>();

        // Check for duplicate review
        var existingReview = await reviewRepo.GetSingleByAsync(
            r => r.TransactionId == request.TransactionId && r.ReviewerUserId == userId);

        if (existingReview is not null)
        {
            return NaitrustResponse<ReviewResponse>.Conflict("You have already submitted a review for this transaction.");
        }

        var review = new Review
        {
            Id = Guid.NewGuid(),
            TransactionId = request.TransactionId,
            ReviewerUserId = userId,
            RevieweeSubjectType = SubjectType.User,
            RevieweeSubjectId = transaction.CreatedByUserId == userId
                ? transaction.CreatedByUserId // Fallback: in a real scenario, get the counterparty
                : transaction.CreatedByUserId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await reviewRepo.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();

        // Update the reviewee's reputation counters
        await UpdateReputationCountersInternalAsync(review.RevieweeSubjectId);

        var response = new ReviewResponse(
            review.Id,
            review.TransactionId,
            review.ReviewerUserId,
            review.Rating,
            review.Comment,
            review.CreatedAt);

        return NaitrustResponse<ReviewResponse>.Created("Review submitted successfully.", response);
    }

    public async Task<NaitrustResponse<bool>> UpdateReputationCountersAsync(Guid userId, CancellationToken ct = default)
    {
        await UpdateReputationCountersInternalAsync(userId);
        return NaitrustResponse<bool>.Success("Reputation counters updated successfully.", true);
    }

    private async Task UpdateReputationCountersInternalAsync(Guid subjectId)
    {
        var reviewRepo = _unitOfWork.GetRepository<Review>();
        var profileRepo = _unitOfWork.GetRepository<ReputationProfile>();

        var profile = await profileRepo.GetSingleByAsync(p => p.SubjectId == subjectId && !p.IsDeleted);

        if (profile is null)
        {
            profile = new ReputationProfile
            {
                Id = Guid.NewGuid(),
                SubjectType = SubjectType.User,
                SubjectId = subjectId,
                IsActive = true
            };
            await profileRepo.AddAsync(profile);
        }

        var reviews = await reviewRepo.GetAllDataAsync(r => r.RevieweeSubjectId == subjectId);
        var reviewList = reviews.ToList();

        profile.RatingCount = reviewList.Count;
        profile.RatingAverage = reviewList.Count > 0
            ? (decimal)reviewList.Average(r => r.Rating)
            : 0;

        profile.UpdatedAt = DateTime.UtcNow;
        await profileRepo.UpdateAsync(profile);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<ReputationProfile> GetOrCreateProfileAsync(Guid userId)
    {
        var repo = _unitOfWork.GetRepository<ReputationProfile>();

        var profile = await repo.GetSingleByAsync(p => p.SubjectId == userId && !p.IsDeleted);

        if (profile is not null)
        {
            return profile;
        }

        profile = new ReputationProfile
        {
            Id = Guid.NewGuid(),
            SubjectType = SubjectType.User,
            SubjectId = userId,
            CompletedTransactionsCount = 0,
            DisputedTransactionsCount = 0,
            CancelledTransactionsCount = 0,
            TotalCompletedValue = 0,
            RatingAverage = 0,
            RatingCount = 0,
            IsActive = true
        };

        await repo.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        return profile;
    }

    private static ReputationProfileResponse MapToResponse(ReputationProfile profile)
    {
        return new ReputationProfileResponse(
            profile.Id,
            profile.SubjectType.ToString(),
            profile.SubjectId,
            profile.CompletedTransactionsCount,
            profile.DisputedTransactionsCount,
            profile.CancelledTransactionsCount,
            profile.TotalCompletedValue,
            profile.RatingAverage,
            profile.RatingCount);
    }
}
