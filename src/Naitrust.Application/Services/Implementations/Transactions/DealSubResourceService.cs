using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Transactions;

public class DealSubResourceService : IDealSubResourceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public DealSubResourceService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    // ============================
    // Messages
    // ============================

    public async Task<NaitrustResponse<List<DealMessageResponse>>> GetMessagesAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealMessage>();
        var messages = await repo.GetAllDataAsync(m => m.DealId == dealId && !m.IsDeleted);

        var items = messages.OrderBy(m => m.CreatedAt).Select(m => new DealMessageResponse(
            m.Id,
            dealId,
            m.SenderUserId,
            m.SenderName,
            m.SenderUserId == userId,
            m.Body,
            m.CreatedAt)).ToList();

        return NaitrustResponse<List<DealMessageResponse>>.Success("Messages retrieved.", items);
    }

    public async Task<NaitrustResponse<DealMessageResponse>> SendMessageAsync(Guid dealId, Guid userId, SendDealMessageRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var senderName = user is not null ? $"{user.FirstName} {user.LastName}".Trim() : "Unknown";

        var repo = _unitOfWork.GetRepository<DealMessage>();
        var message = new DealMessage
        {
            DealId = dealId,
            SenderUserId = userId,
            SenderName = senderName,
            Body = request.Body,
            IsActive = true
        };

        await repo.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DealMessageResponse>.Created("Message sent.", new DealMessageResponse(
            message.Id,
            dealId,
            message.SenderUserId,
            message.SenderName,
            true,
            message.Body,
            message.CreatedAt));
    }

    // ============================
    // Tracking (Milestones)
    // ============================

    public async Task<NaitrustResponse<List<TrackingMilestoneResponse>>> GetTrackingAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var milestones = await GetOrderedMilestonesAsync(dealId);
        var responses = await MapMilestonesAsync(milestones);
        return NaitrustResponse<List<TrackingMilestoneResponse>>.Success("Tracking data retrieved.", responses);
    }

    public async Task<NaitrustResponse<List<TrackingMilestoneResponse>>> AddTrackingStepAsync(Guid dealId, Guid userId, AddTrackingStepRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Milestone>();
        var milestones = await GetOrderedMilestonesAsync(dealId);

        // Determine sort order
        int sortOrder;
        if (request.AfterStepId.HasValue)
        {
            var afterIdx = milestones.FindIndex(m => m.Id == request.AfterStepId.Value);
            sortOrder = afterIdx >= 0 ? afterIdx + 1 : milestones.Count;
            // Shift subsequent milestones
            for (int i = sortOrder; i < milestones.Count; i++)
            {
                milestones[i].SortOrder = i + 1;
                await repo.UpdateAsync(milestones[i]);
            }
        }
        else
        {
            // Insert before the first non-done milestone
            var insertAt = milestones.FindIndex(m => m.Status != MilestoneStatus.Approved && m.Status != MilestoneStatus.Submitted);
            sortOrder = insertAt >= 0 ? insertAt : milestones.Count;
            for (int i = sortOrder; i < milestones.Count; i++)
            {
                milestones[i].SortOrder = i + 1;
                await repo.UpdateAsync(milestones[i]);
            }
        }

        var milestone = new Milestone
        {
            Id = Guid.NewGuid(),
            DealId = dealId,
            Title = request.Title,
            Description = request.Description,
            SortOrder = sortOrder,
            Status = MilestoneStatus.Approved, // Added steps are completed (done)
            UpdatedByUserId = userId,
            StatusChangedAt = DateTime.UtcNow,
            IsActive = true
        };

        await repo.AddAsync(milestone);
        await _unitOfWork.SaveChangesAsync();

        var updated = await GetOrderedMilestonesAsync(dealId);
        return NaitrustResponse<List<TrackingMilestoneResponse>>.Created("Tracking step added.", await MapMilestonesAsync(updated));
    }

    public async Task<NaitrustResponse<List<TrackingMilestoneResponse>>> AdvanceTrackingAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Milestone>();
        var milestones = await GetOrderedMilestonesAsync(dealId);

        // Find current or first pending
        var currentIdx = milestones.FindIndex(m => m.Status == MilestoneStatus.InProgress);
        var idx = currentIdx >= 0 ? currentIdx : milestones.FindIndex(m => m.Status == MilestoneStatus.Pending);

        if (idx < 0)
        {
            return NaitrustResponse<List<TrackingMilestoneResponse>>.BadRequest("No milestone to advance.");
        }

        milestones[idx].Status = MilestoneStatus.Approved;
        milestones[idx].UpdatedByUserId = userId;
        milestones[idx].StatusChangedAt = DateTime.UtcNow;
        await repo.UpdateAsync(milestones[idx]);

        // Set next pending as current
        int nextIdx = -1;
        for (int i = idx + 1; i < milestones.Count; i++)
        {
            if (milestones[i].Status == MilestoneStatus.Pending)
            {
                nextIdx = i;
                break;
            }
        }
        if (nextIdx >= 0)
        {
            milestones[nextIdx].Status = MilestoneStatus.InProgress;
            await repo.UpdateAsync(milestones[nextIdx]);
        }

        await _unitOfWork.SaveChangesAsync();

        var updated = await GetOrderedMilestonesAsync(dealId);
        return NaitrustResponse<List<TrackingMilestoneResponse>>.Success("Tracking advanced.", await MapMilestonesAsync(updated));
    }

    public async Task<NaitrustResponse<List<TrackingMilestoneResponse>>> RevertTrackingAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Milestone>();
        var milestones = await GetOrderedMilestonesAsync(dealId);

        var currentIdx = milestones.FindIndex(m => m.Status == MilestoneStatus.InProgress);

        if (currentIdx < 0)
        {
            // All done — re-open the last completed one
            for (int i = milestones.Count - 1; i >= 0; i--)
            {
                if (milestones[i].Status == MilestoneStatus.Approved)
                {
                    milestones[i].Status = MilestoneStatus.InProgress;
                    milestones[i].UpdatedByUserId = null;
                    milestones[i].StatusChangedAt = null;
                    await repo.UpdateAsync(milestones[i]);
                    break;
                }
            }
        }
        else if (currentIdx > 0)
        {
            // Current becomes pending, previous becomes current
            milestones[currentIdx].Status = MilestoneStatus.Pending;
            milestones[currentIdx].UpdatedByUserId = null;
            milestones[currentIdx].StatusChangedAt = null;
            await repo.UpdateAsync(milestones[currentIdx]);

            milestones[currentIdx - 1].Status = MilestoneStatus.InProgress;
            milestones[currentIdx - 1].UpdatedByUserId = null;
            milestones[currentIdx - 1].StatusChangedAt = null;
            await repo.UpdateAsync(milestones[currentIdx - 1]);
        }

        await _unitOfWork.SaveChangesAsync();

        var updated = await GetOrderedMilestonesAsync(dealId);
        return NaitrustResponse<List<TrackingMilestoneResponse>>.Success("Tracking reverted.", await MapMilestonesAsync(updated));
    }

    public async Task<NaitrustResponse<List<TrackingMilestoneResponse>>> EditTrackingStepAsync(Guid dealId, Guid stepId, Guid userId, EditTrackingStepRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Milestone>();
        var milestone = await repo.GetByIdAsync(stepId);

        if (milestone is null || milestone.DealId != dealId || milestone.IsDeleted)
        {
            return NaitrustResponse<List<TrackingMilestoneResponse>>.NotFound("Milestone not found.");
        }

        milestone.Title = request.Title;
        milestone.Description = request.Description;
        milestone.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(milestone);
        await _unitOfWork.SaveChangesAsync();

        var updated = await GetOrderedMilestonesAsync(dealId);
        return NaitrustResponse<List<TrackingMilestoneResponse>>.Success("Tracking step updated.", await MapMilestonesAsync(updated));
    }

    private async Task<List<Milestone>> GetOrderedMilestonesAsync(Guid dealId)
    {
        var repo = _unitOfWork.GetRepository<Milestone>();
        var milestones = await repo.GetAllDataAsync(m => m.DealId == dealId && !m.IsDeleted);
        return milestones.OrderBy(m => m.SortOrder).ThenBy(m => m.CreatedAt).ToList();
    }

    private async Task<List<TrackingMilestoneResponse>> MapMilestonesAsync(List<Milestone> milestones)
    {
        var responses = new List<TrackingMilestoneResponse>();
        foreach (var m in milestones)
        {
            // Frontend status: 'done' | 'current' | 'pending'
            var status = m.Status switch
            {
                MilestoneStatus.Approved => "done",
                MilestoneStatus.Submitted => "done",
                MilestoneStatus.InProgress => "current",
                _ => "pending"
            };

            string? updatedByName = null;
            if (status == "done" && m.UpdatedByUserId.HasValue)
            {
                var user = await _userManager.FindByIdAsync(m.UpdatedByUserId.Value.ToString());
                updatedByName = user is not null ? $"{user.FirstName} {user.LastName}".Trim() : null;
            }

            responses.Add(new TrackingMilestoneResponse(m.Id, m.Title, m.Description, status, updatedByName, m.StatusChangedAt));
        }
        return responses;
    }

    // ============================
    // Termination
    // ============================

    public async Task<NaitrustResponse<DealTerminationResponse?>> GetTerminationAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealTermination>();
        var termination = await repo.GetSingleByAsync(t => t.DealId == dealId && !t.IsDeleted);

        if (termination is null)
        {
            return NaitrustResponse<DealTerminationResponse?>.Success("No termination request found.", null);
        }

        return NaitrustResponse<DealTerminationResponse?>.Success("Termination request retrieved.", await MapTerminationResponse(termination, userId));
    }

    public async Task<NaitrustResponse<DealTerminationResponse>> RequestTerminationAsync(Guid dealId, Guid userId, RequestTerminationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealTermination>();
        var existing = await repo.GetSingleByAsync(t => t.DealId == dealId && !t.IsDeleted && (t.Status == "requested" || t.Status == "accepted"));

        if (existing is not null)
        {
            return NaitrustResponse<DealTerminationResponse>.BadRequest("A termination request already exists for this deal.");
        }

        var termination = new DealTermination
        {
            DealId = dealId,
            RequestedByUserId = userId,
            Reason = request.Reason,
            Status = "requested",
            IsActive = true
        };

        await repo.AddAsync(termination);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DealTerminationResponse>.Created("Termination requested.", await MapTerminationResponse(termination, userId));
    }

    public async Task<NaitrustResponse<DealTerminationResponse>> RespondToTerminationAsync(Guid dealId, Guid userId, RespondTerminationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealTermination>();
        var termination = await repo.GetSingleByAsync(t => t.DealId == dealId && !t.IsDeleted && t.Status == "requested");

        if (termination is null)
        {
            return NaitrustResponse<DealTerminationResponse>.NotFound("No pending termination request found.");
        }

        termination.Status = request.Accept ? "accepted" : "rejected";
        termination.RespondedByUserId = userId;
        termination.RespondedAt = DateTime.UtcNow;
        termination.ResponseReason = request.Accept ? null : request.Reason;

        await repo.UpdateAsync(termination);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DealTerminationResponse>.Success("Termination response recorded.", await MapTerminationResponse(termination, userId));
    }

    private async Task<DealTerminationResponse> MapTerminationResponse(DealTermination t, Guid currentUserId)
    {
        var requestedByUser = await _userManager.FindByIdAsync(t.RequestedByUserId.ToString());
        var requestedByName = t.RequestedByUserId == currentUserId ? "You"
            : requestedByUser is not null ? $"{requestedByUser.FirstName} {requestedByUser.LastName}".Trim()
            : "Unknown";

        string? respondedByName = null;
        if (t.RespondedByUserId.HasValue)
        {
            var respondedByUser = await _userManager.FindByIdAsync(t.RespondedByUserId.Value.ToString());
            respondedByName = t.RespondedByUserId.Value == currentUserId ? "You"
                : respondedByUser is not null ? $"{respondedByUser.FirstName} {respondedByUser.LastName}".Trim()
                : "Counterparty";
        }

        return new DealTerminationResponse(
            t.DealId,
            t.Status,
            t.Reason,
            requestedByName,
            t.RequestedByUserId == currentUserId,
            t.CreatedAt,
            respondedByName,
            t.RespondedAt,
            t.ResponseReason);
    }
}
