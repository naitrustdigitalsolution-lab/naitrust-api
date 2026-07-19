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

    public DisputeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<DisputeResponse>> OpenDisputeAsync(Guid userId, OpenDisputeRequest request, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(request.TransactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<DisputeResponse>.NotFound("Transaction not found.");
        }

        var repo = _unitOfWork.GetRepository<Dispute>();

        var dispute = new Dispute
        {
            Id = Guid.NewGuid(),
            TransactionId = request.TransactionId,
            OpenedByUserId = userId,
            Status = DisputeStatus.Opened,
            Reason = request.Reason,
            Description = request.Description,
            IsActive = true
        };

        await repo.AddAsync(dispute);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DisputeResponse>.Created(
            "Dispute opened successfully.",
            MapToResponse(dispute, null));
    }

    public async Task<NaitrustResponse<DisputeResponse>> GetDisputeAsync(Guid disputeId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Dispute>();
        var dispute = await repo.GetByIdAsync(disputeId);

        if (dispute is null || dispute.IsDeleted)
        {
            return NaitrustResponse<DisputeResponse>.NotFound("Dispute not found.");
        }

        var messageRepo = _unitOfWork.GetRepository<DisputeMessage>();
        var messages = await messageRepo.GetAllDataAsync(
            m => m.DisputeId == disputeId,
            orderBy: q => q.OrderBy(m => m.CreatedAt));

        var messageResponses = messages.Select(m => new DisputeMessageResponse(
            m.Id,
            m.SenderUserId,
            m.Message,
            m.CreatedAt)).ToList();

        return NaitrustResponse<DisputeResponse>.Success(
            "Dispute retrieved successfully.",
            MapToResponse(dispute, messageResponses));
    }

    public async Task<NaitrustResponse<List<DisputeResponse>>> ListDisputesByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Dispute>();
        var disputes = await repo.GetAllDataAsync(
            d => d.TransactionId == transactionId && !d.IsDeleted,
            orderBy: q => q.OrderByDescending(d => d.CreatedAt));

        var responses = disputes.Select(d => MapToResponse(d, null)).ToList();

        return NaitrustResponse<List<DisputeResponse>>.Success(
            "Disputes retrieved successfully.", responses);
    }

    public async Task<NaitrustResponse<DisputeMessageResponse>> AddMessageAsync(Guid disputeId, Guid userId, AddDisputeMessageRequest request, CancellationToken ct = default)
    {
        var disputeRepo = _unitOfWork.GetRepository<Dispute>();
        var dispute = await disputeRepo.GetByIdAsync(disputeId);

        if (dispute is null || dispute.IsDeleted)
        {
            return NaitrustResponse<DisputeMessageResponse>.NotFound("Dispute not found.");
        }

        var messageRepo = _unitOfWork.GetRepository<DisputeMessage>();

        var message = new DisputeMessage
        {
            Id = Guid.NewGuid(),
            DisputeId = disputeId,
            SenderUserId = userId,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        await messageRepo.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();

        var response = new DisputeMessageResponse(
            message.Id,
            message.SenderUserId,
            message.Message,
            message.CreatedAt);

        return NaitrustResponse<DisputeMessageResponse>.Created("Message added successfully.", response);
    }

    public async Task<NaitrustResponse<DisputeEvidenceResponse>> AddEvidenceAsync(Guid disputeId, Guid userId, AddDisputeEvidenceRequest request, CancellationToken ct = default)
    {
        var disputeRepo = _unitOfWork.GetRepository<Dispute>();
        var dispute = await disputeRepo.GetByIdAsync(disputeId);

        if (dispute is null || dispute.IsDeleted)
        {
            return NaitrustResponse<DisputeEvidenceResponse>.NotFound("Dispute not found.");
        }

        var evidenceRepo = _unitOfWork.GetRepository<DisputeEvidence>();

        var evidence = new DisputeEvidence
        {
            Id = Guid.NewGuid(),
            DisputeId = disputeId,
            EvidenceFileId = request.EvidenceFileId,
            SubmittedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await evidenceRepo.AddAsync(evidence);
        await _unitOfWork.SaveChangesAsync();

        var response = new DisputeEvidenceResponse(
            evidence.Id,
            evidence.DisputeId,
            evidence.SubmittedByUserId,
            Description: null,
            FileUrl: null,
            evidence.CreatedAt);

        return NaitrustResponse<DisputeEvidenceResponse>.Created("Evidence added successfully.", response);
    }

    public async Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, Guid userId, ResolveDisputeRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Dispute>();
        var dispute = await repo.GetByIdAsync(disputeId);

        if (dispute is null || dispute.IsDeleted)
        {
            return NaitrustResponse<DisputeResponse>.NotFound("Dispute not found.");
        }

        if (!Enum.TryParse<DisputeResolution>(request.Resolution, ignoreCase: true, out var resolution))
        {
            return NaitrustResponse<DisputeResponse>.BadRequest($"Invalid resolution: {request.Resolution}");
        }

        dispute.Resolution = resolution;
        dispute.ResolvedAt = DateTime.UtcNow;
        dispute.UpdatedAt = DateTime.UtcNow;

        dispute.Status = resolution switch
        {
            DisputeResolution.Release => DisputeStatus.ResolvedRelease,
            DisputeResolution.Refund => DisputeStatus.ResolvedRefund,
            DisputeResolution.Split => DisputeStatus.ResolvedSplit,
            DisputeResolution.Closed => DisputeStatus.Closed,
            _ => DisputeStatus.Closed
        };

        await repo.UpdateAsync(dispute);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<DisputeResponse>.Success(
            "Dispute resolved successfully.",
            MapToResponse(dispute, null));
    }

    private static DisputeResponse MapToResponse(Dispute dispute, List<DisputeMessageResponse>? messages)
    {
        return new DisputeResponse(
            dispute.Id,
            dispute.TransactionId,
            dispute.OpenedByUserId,
            dispute.Status.ToString(),
            dispute.Reason,
            dispute.Description,
            dispute.Resolution?.ToString(),
            dispute.ResolvedAt,
            messages,
            dispute.CreatedAt);
    }
}
