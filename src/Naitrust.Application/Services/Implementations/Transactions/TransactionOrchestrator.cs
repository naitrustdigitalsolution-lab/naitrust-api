using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Transactions;

// The single authority for all transaction state transitions
public class TransactionOrchestrator : ITransactionOrchestrator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public TransactionOrchestrator(IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<NaitrustResponse<TransactionResponse>> InvitePartyAsync(Guid transactionId, Guid userId, InvitePartyRequest request, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.Draft)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in Draft status to invite a party.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        if (!Enum.TryParse<PartyType>(request.PartyType, ignoreCase: true, out var partyType))
        {
            return NaitrustResponse<TransactionResponse>.BadRequest($"Invalid party type: {request.PartyType}");
        }

        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();

        var counterparty = new TransactionParty
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            PartyType = partyType,
            PartyMode = transaction.PartyMode,
            DisplayName = request.DisplayName ?? string.Empty,
            Email = request.Email,
            Phone = request.Phone,
            Status = TransactionPartyStatus.Invited,
            IsActive = true
        };

        await partyRepo.AddAsync(counterparty);

        transaction.Status = TransactionStatus.PendingCounterparty;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Party Invited",
            $"A counterparty has been invited to transaction {transaction.Reference}.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Party invited successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> AcceptInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.PendingCounterparty)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in PendingCounterparty status to accept an invitation.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var party = await partyRepo.GetSingleByAsync(
            p => p.TransactionId == transactionId && p.UserId == userId && !p.IsDeleted);

        if (party is not null)
        {
            party.Status = TransactionPartyStatus.Accepted;
            party.AcceptedAt = DateTime.UtcNow;
            await partyRepo.UpdateAsync(party);
        }

        transaction.Status = TransactionStatus.TermsNegotiation;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            transaction.CreatedByUserId, "Invitation Accepted",
            $"The invitation to transaction {transaction.Reference} has been accepted.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Invitation accepted successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> RejectInvitationAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.PendingCounterparty)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in PendingCounterparty status to reject an invitation.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var party = await partyRepo.GetSingleByAsync(
            p => p.TransactionId == transactionId && p.UserId == userId && !p.IsDeleted);

        if (party is not null)
        {
            party.Status = TransactionPartyStatus.Rejected;
            await partyRepo.UpdateAsync(party);
        }

        transaction.Status = TransactionStatus.Cancelled;
        transaction.CancelledAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            transaction.CreatedByUserId, "Invitation Rejected",
            $"The invitation to transaction {transaction.Reference} has been rejected.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Invitation rejected successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> ProposeTermsAsync(Guid transactionId, Guid userId, ProposeTermsRequest request, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.TermsNegotiation)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in TermsNegotiation status to propose terms.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        var agreementRepo = _unitOfWork.GetRepository<Agreement>();

        // Determine version by counting existing agreements for this transaction
        var existingAgreements = await agreementRepo.GetAllDataAsync(a => a.TransactionId == transactionId && !a.IsDeleted);
        var nextVersion = existingAgreements.Count() + 1;

        var agreement = new Agreement
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            Version = nextVersion,
            Summary = request.Summary,
            Description = request.Description,
            DeliveryConditions = request.DeliveryConditions,
            ReleaseConditions = request.ReleaseConditions,
            ProofRequirements = request.ProofRequirements,
            DisputeRules = request.DisputeRules,
            AutoConfirmWindowHours = request.AutoConfirmWindowHours,
            DeliveryDueAt = request.DeliveryDueAt,
            CreatedByUserId = userId,
            IsActive = true
        };

        await agreementRepo.AddAsync(agreement);

        transaction.AgreementId = agreement.Id;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Terms Proposed",
            $"New terms have been proposed for transaction {transaction.Reference}.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Terms proposed successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> ApproveTermsAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.TermsNegotiation)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in TermsNegotiation status to approve terms.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        // Freeze the agreement if one exists
        if (transaction.AgreementId.HasValue)
        {
            var agreementRepo = _unitOfWork.GetRepository<Agreement>();
            var agreement = await agreementRepo.GetByIdAsync(transaction.AgreementId.Value);
            if (agreement is not null)
            {
                agreement.FrozenAt = DateTime.UtcNow;
                await agreementRepo.UpdateAsync(agreement);
            }
        }

        transaction.Status = TransactionStatus.AwaitingFunding;
        transaction.TermsAcceptedAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Terms Approved",
            $"Terms have been approved for transaction {transaction.Reference}.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Terms approved successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> InitiateFundingAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.AwaitingFunding)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in AwaitingFunding status to initiate funding.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        // Stub: just transition the status
        transaction.Status = TransactionStatus.Funded;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Funding Initiated",
            $"Funding has been initiated for transaction {transaction.Reference}.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Funding initiated successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> SubmitDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.Funded)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in Funded status to submit delivery.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        transaction.Status = TransactionStatus.InProgress;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Delivery Submitted",
            $"Delivery has been submitted for transaction {transaction.Reference}.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Delivery submitted successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> ConfirmDeliveryAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.InProgress)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction must be in InProgress status to confirm delivery.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        transaction.Status = TransactionStatus.Completed;
        transaction.CompletedAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Delivery Confirmed",
            $"Delivery has been confirmed for transaction {transaction.Reference}. Transaction completed.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Delivery confirmed successfully.",
            await BuildTransactionResponse(transaction));
    }

    public async Task<NaitrustResponse<TransactionResponse>> CancelTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();
        var transaction = await transactionRepo.GetByIdAsync(transactionId);

        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionResponse>.NotFound("Transaction not found.");
        }

        // Cannot cancel already terminal states
        if (transaction.Status == TransactionStatus.Cancelled || transaction.Status == TransactionStatus.Completed)
        {
            return NaitrustResponse<TransactionResponse>.BadRequest("Transaction is already in a terminal status and cannot be cancelled.");
        }

        if (!await IsUserPartyToTransaction(transactionId, userId))
        {
            return NaitrustResponse<TransactionResponse>.Forbidden("You are not a party to this transaction.");
        }

        transaction.Status = TransactionStatus.Cancelled;
        transaction.CancelledAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        await transactionRepo.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Transaction Cancelled",
            $"Transaction {transaction.Reference} has been cancelled.",
            "TransactionUpdate", null, ct);

        return NaitrustResponse<TransactionResponse>.Success(
            "Transaction cancelled successfully.",
            await BuildTransactionResponse(transaction));
    }

    private async Task<bool> IsUserPartyToTransaction(Guid transactionId, Guid userId)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var party = await partyRepo.GetSingleByAsync(
            p => p.TransactionId == transactionId && p.UserId == userId && !p.IsDeleted);
        return party is not null;
    }

    private async Task<TransactionResponse> BuildTransactionResponse(Transaction transaction)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.TransactionId == transaction.Id && !p.IsDeleted);
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

        return MapToResponse(transaction, partyResponses, agreementResponse, allowedActions);
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
