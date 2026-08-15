using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Disputes;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Disputes;

public class DisputeService : IDisputeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public DisputeService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<DisputeResponse?>> GetByTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Dispute>();
        var dispute = await repo.GetSingleByAsync(d => d.DealId == transactionId && !d.IsDeleted);

        if (dispute is null)
        {
            return NaitrustResponse<DisputeResponse?>.Success("No dispute found.", null);
        }

        var messages = await GetDisputeMessagesAsync(dispute.Id, userId);
        return NaitrustResponse<DisputeResponse?>.Success("Dispute retrieved.", await MapToResponse(dispute, messages, userId));
    }

    public async Task<NaitrustResponse<DisputeResponse>> OpenDisputeAsync(Guid transactionId, Guid userId, OpenDisputeRequest request, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(transactionId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DisputeResponse>.NotFound("Deal not found.");
        }

        var repo = _unitOfWork.GetRepository<Dispute>();
        var existing = await repo.GetSingleByAsync(d => d.DealId == transactionId && !d.IsDeleted
            && (d.Status == DisputeStatus.Opened || d.Status == DisputeStatus.UnderReview));

        if (existing is not null)
        {
            return NaitrustResponse<DisputeResponse>.BadRequest("A dispute is already open for this transaction.");
        }

        var dispute = new Dispute
        {
            Id = Guid.NewGuid(),
            DealId = transactionId,
            OpenedByUserId = userId,
            Status = request.HasEvidence ? DisputeStatus.UnderReview : DisputeStatus.EvidenceRequested,
            Reason = request.Reason,
            Description = request.Description,
            HasEvidence = request.HasEvidence,
            IsActive = true
        };

        await repo.AddAsync(dispute);

        // Auto-add initial support message
        var msgRepo = _unitOfWork.GetRepository<DisputeMessage>();
        var autoMsg = new DisputeMessage
        {
            Id = Guid.NewGuid(),
            DisputeId = dispute.Id,
            SenderUserId = Guid.Empty, // System
            Message = request.HasEvidence
                ? "Your report and evidence were received. Automatic payment release is frozen while the dispute is reviewed."
                : "Your report was received without evidence. Payment is not frozen yet. Upload relevant evidence as soon as possible; insufficient evidence may affect the final decision.",
            CreatedAt = DateTime.UtcNow
        };
        await msgRepo.AddAsync(autoMsg);

        await _unitOfWork.SaveChangesAsync();

        var messages = await GetDisputeMessagesAsync(dispute.Id, userId);
        return NaitrustResponse<DisputeResponse>.Created("Dispute opened.", await MapToResponse(dispute, messages, userId));
    }

    public async Task<NaitrustResponse<DisputeResponse>> AddMessageToTransactionDisputeAsync(Guid transactionId, Guid userId, AddDisputeMessageRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Dispute>();
        var dispute = await repo.GetSingleByAsync(d => d.DealId == transactionId && !d.IsDeleted);

        if (dispute is null)
        {
            return NaitrustResponse<DisputeResponse>.NotFound("No dispute found for this transaction.");
        }

        var msgRepo = _unitOfWork.GetRepository<DisputeMessage>();
        var message = new DisputeMessage
        {
            Id = Guid.NewGuid(),
            DisputeId = dispute.Id,
            SenderUserId = userId,
            Message = request.Body,
            CreatedAt = DateTime.UtcNow
        };

        await msgRepo.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();

        var messages = await GetDisputeMessagesAsync(dispute.Id, userId);
        return NaitrustResponse<DisputeResponse>.Created("Message added.", await MapToResponse(dispute, messages, userId));
    }

    private async Task<List<DisputeMessageDto>> GetDisputeMessagesAsync(Guid disputeId, Guid userId)
    {
        var msgRepo = _unitOfWork.GetRepository<DisputeMessage>();
        var messages = await msgRepo.GetAllDataAsync(m => m.DisputeId == disputeId);
        var ordered = messages.OrderBy(m => m.CreatedAt).ToList();

        var result = new List<DisputeMessageDto>();
        foreach (var m in ordered)
        {
            string byName;
            if (m.SenderUserId == Guid.Empty)
            {
                byName = "Naitrust Support";
            }
            else if (m.SenderUserId == userId)
            {
                byName = "You";
            }
            else
            {
                var user = await _userManager.FindByIdAsync(m.SenderUserId.ToString());
                byName = user is not null ? $"{user.FirstName} {user.LastName}".Trim() : "Unknown";
            }

            result.Add(new DisputeMessageDto(m.Id, byName, m.SenderUserId == userId, m.Message, m.CreatedAt));
        }

        return result;
    }

    private async Task<DisputeResponse> MapToResponse(Dispute dispute, List<DisputeMessageDto> messages, Guid userId)
    {
        string openedByName;
        if (dispute.OpenedByUserId == userId)
        {
            openedByName = "You";
        }
        else
        {
            var user = await _userManager.FindByIdAsync(dispute.OpenedByUserId.ToString());
            openedByName = user is not null ? $"{user.FirstName} {user.LastName}".Trim() : "Unknown";
        }

        // Frontend status: 'awaiting_evidence' | 'open' | 'under_review' | 'resolved_release' | 'resolved_refund'
        var status = dispute.Status switch
        {
            DisputeStatus.Opened => "open",
            DisputeStatus.EvidenceRequested => "awaiting_evidence",
            DisputeStatus.UnderReview => "under_review",
            DisputeStatus.ResolvedRelease => "resolved_release",
            DisputeStatus.ResolvedRefund => "resolved_refund",
            _ => dispute.Status.ToString().ToLowerInvariant()
        };

        return new DisputeResponse(
            dispute.DealId,
            status,
            dispute.Reason,
            dispute.Description ?? "",
            openedByName,
            dispute.CreatedAt,
            messages,
            AddBusinessDays(dispute.CreatedAt, 2));
    }

    /// <summary>Adds a number of business days (Mon-Fri) to a UTC timestamp, skipping weekends.</summary>
    private static DateTime AddBusinessDays(DateTime start, int businessDays)
    {
        var date = start;
        var added = 0;
        while (added < businessDays)
        {
            date = date.AddDays(1);
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                added++;
            }
        }
        return date;
    }
}
