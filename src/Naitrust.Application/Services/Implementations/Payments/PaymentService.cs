using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Payments;
using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Payments;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILedgerService _ledgerService;

    public PaymentService(IUnitOfWork unitOfWork, ILedgerService ledgerService)
    {
        _unitOfWork = unitOfWork;
        _ledgerService = ledgerService;
    }

    public async Task<NaitrustResponse<VirtualAccountResponse>> CreateSettlementAccountAsync(Guid userId, CreateSettlementAccountRequest request, CancellationToken ct = default)
    {
        if (!Enum.TryParse<PaymentPartnerId>(request.PartnerId, ignoreCase: true, out var partner))
        {
            return NaitrustResponse<VirtualAccountResponse>.BadRequest($"Invalid partner: {request.PartnerId}");
        }

        var repo = _unitOfWork.GetRepository<VirtualAccount>();

        // Check if a settlement account already exists for this user/business
        VirtualAccount? existing;
        if (request.BusinessId.HasValue)
        {
            existing = await repo.GetSingleByAsync(va => va.BusinessId == request.BusinessId && va.Type == VirtualAccountType.Settlement);
        }
        else
        {
            existing = await repo.GetSingleByAsync(va => va.UserId == userId && va.BusinessId == null && va.Type == VirtualAccountType.Settlement);
        }

        if (existing is not null)
        {
            return NaitrustResponse<VirtualAccountResponse>.Conflict("Settlement account already exists.");
        }

        var virtualAccount = new VirtualAccount
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BusinessId = request.BusinessId,
            Type = VirtualAccountType.Settlement,
            Partner = partner,
            AmountReceivedMinor = 0,
            Currency = "NGN",
            Status = VirtualAccountStatus.Requested,
            IsActive = true
        };

        await repo.AddAsync(virtualAccount);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<VirtualAccountResponse>.Created(
            "Settlement account created successfully.",
            MapToVirtualAccountResponse(virtualAccount));
    }

    public async Task<NaitrustResponse<VirtualAccountResponse>> GetSettlementAccountAsync(Guid userId, Guid? businessId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VirtualAccount>();

        VirtualAccount? account;
        if (businessId.HasValue)
        {
            account = await repo.GetSingleByAsync(va => va.BusinessId == businessId && va.Type == VirtualAccountType.Settlement);
        }
        else
        {
            account = await repo.GetSingleByAsync(va => va.UserId == userId && va.BusinessId == null && va.Type == VirtualAccountType.Settlement);
        }

        if (account is null)
        {
            return NaitrustResponse<VirtualAccountResponse>.NotFound("Settlement account not found.");
        }

        return NaitrustResponse<VirtualAccountResponse>.Success("Settlement account retrieved successfully.", MapToVirtualAccountResponse(account));
    }

    public async Task<NaitrustResponse<PaymentStatusResponse>> GetPaymentStatusAsync(Guid transactionId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(transactionId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<PaymentStatusResponse>.NotFound("Deal not found.");
        }

        // Compute escrow balance from ledger
        var ledgerRepo = _unitOfWork.GetRepository<LedgerEntry>();
        var ledgerEntries = await ledgerRepo.GetAllDataAsync(e => e.TransactionId == transactionId);
        var entryList = ledgerEntries.ToList();

        long escrowBalance = 0;
        LedgerSummaryDto? ledgerSummary = null;
        if (entryList.Count > 0)
        {
            var totalDebit = entryList.Sum(e => e.DebitMinor);
            var totalCredit = entryList.Sum(e => e.CreditMinor);
            escrowBalance = totalDebit - totalCredit;
            ledgerSummary = new LedgerSummaryDto(totalDebit, totalCredit, deal.Currency);
        }

        var response = new PaymentStatusResponse(
            transactionId,
            deal.PaymentStatus.ToString(),
            escrowBalance,
            ledgerSummary);

        return NaitrustResponse<PaymentStatusResponse>.Success("Payment status retrieved successfully.", response);
    }

    public async Task<NaitrustResponse<ReleaseRequestResponse>> RequestReleaseAsync(Guid transactionId, RequestReleaseRequest request, CancellationToken ct = default)
    {
        var dealRepo2 = _unitOfWork.GetRepository<Deal>();
        var deal2 = await dealRepo2.GetByIdAsync(transactionId);

        if (deal2 is null || deal2.IsDeleted)
        {
            return NaitrustResponse<ReleaseRequestResponse>.NotFound("Deal not found.");
        }

        var repo = _unitOfWork.GetRepository<ReleaseRequest>();

        var releaseRequest = new ReleaseRequest
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            RequestedByUserId = deal2.CreatedByUserId,
            Status = ReleaseRequestStatus.Requested,
            Reason = request.Reason,
            RequestedAt = DateTime.UtcNow,
            IsActive = true
        };

        await repo.AddAsync(releaseRequest);
        await _unitOfWork.SaveChangesAsync();

        var response = new ReleaseRequestResponse(
            releaseRequest.Id,
            releaseRequest.TransactionId,
            releaseRequest.RequestedByUserId,
            releaseRequest.Status.ToString(),
            deal2.AmountMinor,
            releaseRequest.Reason,
            releaseRequest.CreatedAt);

        return NaitrustResponse<ReleaseRequestResponse>.Created("Release request submitted successfully.", response);
    }

    public async Task<NaitrustResponse<PaginatedResponse<LedgerEntryResponse>>> GetLedgerAsync(Guid transactionId, PaginationRequest pagination, CancellationToken ct = default)
    {
        var ledgerResult = await _ledgerService.GetEntriesByTransactionAsync(transactionId, ct);

        if (!ledgerResult.IsSuccessful || ledgerResult.Data is null)
        {
            return NaitrustResponse<PaginatedResponse<LedgerEntryResponse>>.Success(
                "Ledger entries retrieved successfully.",
                new PaginatedResponse<LedgerEntryResponse>(new List<LedgerEntryResponse>(), pagination.Page, pagination.PageSize, 0, 0));
        }

        var allEntries = ledgerResult.Data;
        var totalCount = allEntries.Count;

        var pagedEntries = allEntries
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<LedgerEntryResponse>>.Success(
            "Ledger entries retrieved successfully.",
            new PaginatedResponse<LedgerEntryResponse>(pagedEntries, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<ReconciliationStatusResponse>> GetReconciliationStatusAsync(Guid transactionId, CancellationToken ct = default)
    {
        var dealRepo3 = _unitOfWork.GetRepository<Deal>();
        var deal3 = await dealRepo3.GetByIdAsync(transactionId);

        if (deal3 is null || deal3.IsDeleted)
        {
            return NaitrustResponse<ReconciliationStatusResponse>.NotFound("Deal not found.");
        }

        var ledgerRepo = _unitOfWork.GetRepository<LedgerEntry>();
        var entries = await ledgerRepo.GetAllDataAsync(e => e.TransactionId == transactionId);
        var entryList = entries.ToList();

        var ledgerBalance = entryList.Sum(e => e.DebitMinor) - entryList.Sum(e => e.CreditMinor);

        // Stub: partner balance comparison not yet implemented
        var response = new ReconciliationStatusResponse(
            transactionId,
            Matches: true,
            LedgerBalance: ledgerBalance,
            PartnerBalance: null,
            LastCheckedAt: DateTime.UtcNow);

        return NaitrustResponse<ReconciliationStatusResponse>.Success("Reconciliation status retrieved successfully.", response);
    }

    public Task<NaitrustResponse<PayoutAccountValidationResponse>> ValidatePayoutAccountAsync(ValidatePayoutAccountRequest request, CancellationToken ct = default)
    {
        // Stub: external partner account validation not yet integrated
        var response = new PayoutAccountValidationResponse(
            AccountName: null,
            NameMatchStatus: NameMatchStatus.Pending.ToString());

        return Task.FromResult(
            NaitrustResponse<PayoutAccountValidationResponse>.Success("Payout account validation submitted.", response));
    }

    private static VirtualAccountResponse MapToVirtualAccountResponse(VirtualAccount va)
    {
        return new VirtualAccountResponse(
            va.Id,
            va.AccountNumber,
            va.AccountName,
            va.BankName,
            va.Status.ToString(),
            va.Type.ToString(),
            va.AmountReceivedMinor,
            va.Currency,
            va.ExpiresAt);
    }
}
