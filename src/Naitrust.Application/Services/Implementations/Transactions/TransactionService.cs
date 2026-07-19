using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Transactions;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<TransactionResponse>> CreateTransactionAsync(Guid userId, CreateTransactionRequest request, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();

        if (!Enum.TryParse<PartyMode>(request.PartyMode, ignoreCase: true, out var partyMode))
        {
            return NaitrustResponse<TransactionResponse>.BadRequest($"Invalid party mode: {request.PartyMode}");
        }

        if (!Enum.TryParse<TransactionCategory>(request.Category, ignoreCase: true, out var category))
        {
            return NaitrustResponse<TransactionResponse>.BadRequest($"Invalid category: {request.Category}");
        }

        var reference = $"NTR-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Reference = reference,
            TransactionTypeId = request.TransactionTypeId,
            CreatedByUserId = userId,
            PartyMode = partyMode,
            Title = request.Title,
            Description = request.Description,
            Category = category,
            AmountMinor = request.AmountMinor,
            FeeMinor = 0,
            Currency = request.Currency,
            Status = TransactionStatus.Draft,
            PaymentStatus = PaymentStatus.NotStarted,
            IsActive = true
        };

        await transactionRepo.AddAsync(transaction);

        var creatorParty = new TransactionParty
        {
            Id = Guid.NewGuid(),
            TransactionId = transaction.Id,
            UserId = userId,
            PartyType = PartyType.Buyer,
            PartyMode = partyMode,
            DisplayName = string.Empty,
            Status = TransactionPartyStatus.Accepted,
            AcceptedAt = DateTime.UtcNow,
            IsActive = true
        };

        await partyRepo.AddAsync(creatorParty);
        await _unitOfWork.SaveChangesAsync();

        var parties = new List<TransactionPartyResponse> { MapToPartyResponse(creatorParty) };

        return NaitrustResponse<TransactionResponse>.Created(
            "Transaction created successfully.",
            MapToResponse(transaction, parties, null));
    }

    public async Task<NaitrustResponse<TransactionResponse>> GetTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.TransactionId == transactionId && !p.IsDeleted);
        var partyResponses = parties.Select(MapToPartyResponse).ToList();

        AgreementResponse? agreementResponse = null;
        if (transaction.AgreementId.HasValue)
        {
            var agreementRepo = _unitOfWork.GetRepository<Agreement>();
            var agreement = await agreementRepo.GetByIdAsync(transaction.AgreementId.Value);
            if (agreement is not null && !agreement.IsDeleted)
            {
                agreementResponse = MapToAgreementResponse(agreement);
            }
        }

        var allowedActions = GetAllowedActions(transaction.Status);

        return NaitrustResponse<TransactionResponse>.Success(
            "Transaction retrieved successfully.",
            MapToResponse(transaction, partyResponses, agreementResponse, allowedActions));
    }

    public async Task<NaitrustResponse<PaginatedResponse<TransactionResponse>>> ListTransactionsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();

        var userParties = await partyRepo.GetAllDataAsync(p => p.UserId == userId && !p.IsDeleted);
        var transactionIds = userParties.Select(p => p.TransactionId).Distinct().ToList();

        if (!transactionIds.Any())
        {
            return NaitrustResponse<PaginatedResponse<TransactionResponse>>.Success(
                "Transactions retrieved successfully.",
                new PaginatedResponse<TransactionResponse>(new List<TransactionResponse>(), pagination.Page, pagination.PageSize, 0, 0));
        }

        var allTransactions = await transactionRepo.GetAllDataAsync(t => transactionIds.Contains(t.Id) && !t.IsDeleted);
        var totalCount = allTransactions.Count();

        var pagedTransactions = allTransactions
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var transactionResponses = new List<TransactionResponse>();
        foreach (var transaction in pagedTransactions)
        {
            var parties = await partyRepo.GetAllDataAsync(p => p.TransactionId == transaction.Id && !p.IsDeleted);
            var partyResponses = parties.Select(MapToPartyResponse).ToList();
            var allowedActions = GetAllowedActions(transaction.Status);
            transactionResponses.Add(MapToResponse(transaction, partyResponses, null, allowedActions));
        }

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<TransactionResponse>>.Success(
            "Transactions retrieved successfully.",
            new PaginatedResponse<TransactionResponse>(transactionResponses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<TransactionResponse>> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequest request, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.Draft)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction can only be updated while in Draft status.");
        }

        if (request.Title is not null)
        {
            transaction.Title = request.Title;
        }

        if (request.Description is not null)
        {
            transaction.Description = request.Description;
        }

        if (request.AmountMinor.HasValue)
        {
            transaction.AmountMinor = request.AmountMinor.Value;
        }

        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.TransactionId == transactionId && !p.IsDeleted);
        var partyResponses = parties.Select(MapToPartyResponse).ToList();
        var allowedActions = GetAllowedActions(transaction.Status);

        return NaitrustResponse<TransactionResponse>.Success(
            "Transaction updated successfully.",
            MapToResponse(transaction, partyResponses, null, allowedActions));
    }

    public async Task<NaitrustResponse<List<TransactionTypeResponse>>> GetTransactionTypesAsync(CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<TransactionType>();
        var types = await repo.GetAllDataAsync(t => t.IsActive && !t.IsDeleted);

        var responses = types.Select(t => new TransactionTypeResponse(
            t.Id,
            t.Key,
            t.Name,
            t.RequiredVerificationLevel.ToString(),
            t.ReleaseMode.ToString(),
            t.AutoConfirmWindowHours,
            t.IsActive)).ToList();

        return NaitrustResponse<List<TransactionTypeResponse>>.Success(
            "Transaction types retrieved successfully.", responses);
    }

    private static TransactionResponse MapToResponse(
        Transaction transaction,
        List<TransactionPartyResponse>? parties,
        AgreementResponse? agreement,
        List<AllowedActionDto>? allowedActions = null)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.Reference,
            transaction.Title,
            transaction.Description,
            transaction.Category.ToString(),
            transaction.AmountMinor,
            transaction.FeeMinor,
            transaction.Currency,
            transaction.Status.ToString(),
            transaction.PaymentStatus.ToString(),
            transaction.PartyMode.ToString(),
            transaction.RiskLevel?.ToString(),
            parties,
            agreement,
            allowedActions,
            transaction.CreatedAt);
    }

    private static TransactionPartyResponse MapToPartyResponse(TransactionParty party)
    {
        return new TransactionPartyResponse(
            party.Id,
            party.UserId,
            party.BusinessId,
            party.PartyType.ToString(),
            party.DisplayName,
            party.Email,
            party.Status.ToString(),
            party.AcceptedAt);
    }

    private static AgreementResponse MapToAgreementResponse(Agreement agreement)
    {
        return new AgreementResponse(
            agreement.Id,
            agreement.Version,
            agreement.Summary,
            agreement.Description,
            agreement.DeliveryConditions,
            agreement.ReleaseConditions,
            agreement.ProofRequirements,
            agreement.DisputeRules,
            agreement.AutoConfirmWindowHours,
            agreement.DeliveryDueAt,
            agreement.FrozenAt,
            agreement.CreatedAt);
    }

    private static List<AllowedActionDto> GetAllowedActions(TransactionStatus status)
    {
        return status switch
        {
            TransactionStatus.Draft => new List<AllowedActionDto>
            {
                new("InviteParty", "Invite Counterparty", true),
                new("Update", "Edit Transaction", true),
                new("Cancel", "Cancel Transaction", true)
            },
            TransactionStatus.PendingCounterparty => new List<AllowedActionDto>
            {
                new("AcceptInvitation", "Accept Invitation", true),
                new("RejectInvitation", "Reject Invitation", true),
                new("Cancel", "Cancel Transaction", true)
            },
            TransactionStatus.TermsNegotiation => new List<AllowedActionDto>
            {
                new("ProposeTerms", "Propose Terms", true),
                new("ApproveTerms", "Approve Terms", true),
                new("Cancel", "Cancel Transaction", true)
            },
            TransactionStatus.AwaitingFunding => new List<AllowedActionDto>
            {
                new("InitiateFunding", "Initiate Funding", true),
                new("Cancel", "Cancel Transaction", true)
            },
            TransactionStatus.Funded => new List<AllowedActionDto>
            {
                new("SubmitDelivery", "Submit Delivery", true),
                new("Cancel", "Cancel Transaction", true)
            },
            TransactionStatus.InProgress => new List<AllowedActionDto>
            {
                new("ConfirmDelivery", "Confirm Delivery", true),
                new("Cancel", "Cancel Transaction", true)
            },
            TransactionStatus.Completed => new List<AllowedActionDto>(),
            TransactionStatus.Cancelled => new List<AllowedActionDto>(),
            _ => new List<AllowedActionDto>()
        };
    }
}
