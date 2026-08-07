using System.Text.Json;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Verification;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Disputes;
using Naitrust.Domain.Models.Enums.Verification;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Admin;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<PaginatedResponse<DealResponse>>> GetDealsAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Deal>();
        var allDeals = await repo.GetAllDataAsync(
            t => !t.IsDeleted,
            orderBy: q => q.OrderByDescending(t => t.CreatedAt));

        var dealList = allDeals.ToList();
        var totalCount = dealList.Count;

        var pagedDeals = dealList
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var responses = pagedDeals.Select(t => new DealResponse(
            t.Id,
            t.Reference,
            t.Title,
            t.Description,
            t.UseCase,
            t.DealType.ToString(),
            t.Category.ToString(),
            t.AmountMinor,
            t.FeeMinor,
            t.Currency,
            t.Status.ToString(),
            t.PaymentStatus.ToString(),
            t.PartyMode.ToString(),
            t.RiskLevel?.ToString(),
            t.DeliveryDueDate,
            t.ReleaseConditions,
            t.ExtendedProductTestingDays,
            t.ExpiresAt,
            t.Recurring,
            t.PreviousReference,
            null,
            null,
            null,
            null,
            t.CreatedAt)).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<DealResponse>>.Success(
            "Deals retrieved successfully.",
            new PaginatedResponse<DealResponse>(responses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<DealResponse>> GetDealAsync(Guid dealId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Deal>();
        var deal = await repo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.DealId == dealId && !p.IsDeleted);

        var partyResponses = parties.Select(p => new DealPartyResponse(
            p.Id,
            p.UserId,
            p.BusinessId,
            p.PartyType.ToString(),
            p.DisplayName,
            p.Email,
            p.Status.ToString(),
            p.AcceptedAt)).ToList();

        AgreementResponse? agreementResponse = null;
        if (deal.AgreementId.HasValue)
        {
            var agreementRepo = _unitOfWork.GetRepository<Agreement>();
            var agreement = await agreementRepo.GetByIdAsync(deal.AgreementId.Value);
            if (agreement is not null && !agreement.IsDeleted)
            {
                var sections = DeserializeSections(agreement.SectionsJson);
                agreementResponse = new AgreementResponse(
                    agreement.Id,
                    agreement.Version,
                    agreement.GeneratedByAi,
                    sections);
            }
        }

        var response = new DealResponse(
            deal.Id,
            deal.Reference,
            deal.Title,
            deal.Description,
            deal.UseCase,
            deal.DealType.ToString(),
            deal.Category.ToString(),
            deal.AmountMinor,
            deal.FeeMinor,
            deal.Currency,
            deal.Status.ToString(),
            deal.PaymentStatus.ToString(),
            deal.PartyMode.ToString(),
            deal.RiskLevel?.ToString(),
            deal.DeliveryDueDate,
            deal.ReleaseConditions,
            deal.ExtendedProductTestingDays,
            deal.ExpiresAt,
            deal.Recurring,
            deal.PreviousReference,
            partyResponses,
            agreementResponse,
            null,
            null,
            deal.CreatedAt);

        return NaitrustResponse<DealResponse>.Success("Deal retrieved successfully.", response);
    }

    public async Task<NaitrustResponse<PaginatedResponse<DisputeResponse>>> GetDisputesAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Dispute>();
        var allDisputes = await repo.GetAllDataAsync(
            d => !d.IsDeleted,
            orderBy: q => q.OrderByDescending(d => d.CreatedAt));

        var disputeList = allDisputes.ToList();
        var totalCount = disputeList.Count;

        var pagedDisputes = disputeList
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var responses = pagedDisputes.Select(d => new DisputeResponse(
            d.DealId,
            d.Status.ToString(),
            d.Reason,
            d.Description ?? "",
            "Admin", // openedByName — admin context
            d.CreatedAt,
            null)).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<DisputeResponse>>.Success(
            "Disputes retrieved successfully.",
            new PaginatedResponse<DisputeResponse>(responses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, ResolveAdminDisputeRequest request, CancellationToken ct = default)
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

        var response = new DisputeResponse(
            dispute.DealId,
            dispute.Status.ToString(),
            dispute.Reason,
            dispute.Description ?? "",
            "Admin", // openedByName — admin context
            dispute.CreatedAt,
            null);

        return NaitrustResponse<DisputeResponse>.Success("Dispute resolved successfully.", response);
    }

    public async Task<NaitrustResponse<PaginatedResponse<VerificationRequestResponse>>> GetVerificationsAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var allVerifications = await repo.GetAllDataAsync(
            v => !v.IsDeleted,
            orderBy: q => q.OrderByDescending(v => v.CreatedAt));

        var verificationList = allVerifications.ToList();
        var totalCount = verificationList.Count;

        var pagedVerifications = verificationList
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var responses = new List<VerificationRequestResponse>();

        foreach (var v in pagedVerifications)
        {
            var steps = await stepRepo.GetAllDataAsync(s => s.VerificationRequestId == v.Id && !s.IsDeleted);
            var stepResponses = steps.Select(s => new VerificationStepResponse(
                s.Step.ToString(),
                s.Status.ToString(),
                s.Message,
                s.StartedAt,
                s.CompletedAt)).ToList();

            responses.Add(new VerificationRequestResponse(
                v.Id,
                v.SubjectType.ToString(),
                v.SubjectId,
                v.VerificationType.ToString(),
                v.VerificationLevel.ToString(),
                v.Status.ToString(),
                stepResponses,
                v.CreatedAt));
        }

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<VerificationRequestResponse>>.Success(
            "Verification requests retrieved successfully.",
            new PaginatedResponse<VerificationRequestResponse>(responses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> UpdateVerificationAsync(Guid verificationId, UpdateAdminVerificationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verification = await repo.GetByIdAsync(verificationId);

        if (verification is null || verification.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        if (!Enum.TryParse<VerificationStatus>(request.Status, ignoreCase: true, out var status))
        {
            return NaitrustResponse<VerificationRequestResponse>.BadRequest($"Invalid status: {request.Status}");
        }

        verification.Status = status;
        verification.ResultSummary = request.ReviewNotes;
        verification.ReviewedAt = DateTime.UtcNow;
        verification.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(verification);
        await _unitOfWork.SaveChangesAsync();

        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var steps = await stepRepo.GetAllDataAsync(s => s.VerificationRequestId == verificationId && !s.IsDeleted);
        var stepResponses = steps.Select(s => new VerificationStepResponse(
            s.Step.ToString(),
            s.Status.ToString(),
            s.Message,
            s.StartedAt,
            s.CompletedAt)).ToList();

        var response = new VerificationRequestResponse(
            verification.Id,
            verification.SubjectType.ToString(),
            verification.SubjectId,
            verification.VerificationType.ToString(),
            verification.VerificationLevel.ToString(),
            verification.Status.ToString(),
            stepResponses,
            verification.CreatedAt);

        return NaitrustResponse<VerificationRequestResponse>.Success("Verification updated successfully.", response);
    }

    public async Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<AuditLog>();
        var allLogs = await repo.GetAllDataAsync(
            orderBy: q => q.OrderByDescending(a => a.CreatedAt));

        var logList = allLogs.ToList();
        var totalCount = logList.Count;

        var pagedLogs = logList
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var responses = pagedLogs.Select(a => new AuditLogResponse(
            a.Id,
            a.ActorUserId,
            a.Action,
            a.EntityType,
            a.EntityId,
            a.IpAddress,
            a.CreatedAt)).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<AuditLogResponse>>.Success(
            "Audit logs retrieved successfully.",
            new PaginatedResponse<AuditLogResponse>(responses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    private static List<AgreementSectionResponse> DeserializeSections(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<AgreementSectionResponse>();
        try
        {
            return JsonSerializer.Deserialize<List<AgreementSectionResponse>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<AgreementSectionResponse>();
        }
        catch
        {
            return new List<AgreementSectionResponse>();
        }
    }
}
