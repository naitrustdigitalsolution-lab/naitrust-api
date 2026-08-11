using Newtonsoft.Json;
using Naitrust.Application.ExternalServices;
using Naitrust.Application.ExternalServices.Anchor;
using Naitrust.Application.Services.Implementations.Invitations;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Transactions;

// The single authority for all deal state transitions
public class DealOrchestrator : IDealOrchestrator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly AnchorPaymentPartner _anchor;

    public DealOrchestrator(
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        AnchorPaymentPartner anchor)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _anchor = anchor;
    }

    public async Task<NaitrustResponse<DealResponse>> InvitePartyAsync(Guid dealId, Guid userId, InvitePartyRequest request, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.Draft)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in Draft status to invite a party.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        if (!Enum.TryParse<PartyType>(request.PartyType, ignoreCase: true, out var partyType))
        {
            return NaitrustResponse<DealResponse>.BadRequest($"Invalid party type: {request.PartyType}");
        }

        var partyRepo = _unitOfWork.GetRepository<DealParty>();

        var counterparty = new DealParty
        {
            Id = Guid.NewGuid(),
            DealId = dealId,
            PartyType = partyType,
            PartyMode = deal.PartyMode,
            DisplayName = request.DisplayName ?? string.Empty,
            Email = request.Email,
            Phone = request.Phone,
            Status = DealPartyStatus.Invited,
            IsActive = true
        };

        await partyRepo.AddAsync(counterparty);

        // Create a DealInvitation with a public token
        var invitationRepo = _unitOfWork.GetRepository<DealInvitation>();
        var publicToken = InvitationService.GeneratePublicToken();

        var creatorParty = await partyRepo.GetSingleByAsync(
            p => p.DealId == dealId && p.UserId == userId && !p.IsDeleted);
        var fromRole = creatorParty?.PartyType.ToString().ToLowerInvariant() ?? "buyer";
        var yourRole = fromRole == "buyer" ? "seller" : "buyer";

        var invitation = new DealInvitation
        {
            Id = Guid.NewGuid(),
            DealId = dealId,
            PublicToken = publicToken,
            IntendedContact = request.Email ?? request.Phone,
            InviterProfileId = userId,
            FromName = creatorParty?.DisplayName ?? string.Empty,
            FromRole = fromRole,
            YourRole = yourRole,
            PartyMode = deal.PartyMode.ToString(),
            Title = deal.Title,
            AmountMinor = deal.AmountMinor,
            Currency = deal.Currency,
            Status = InvitationStatus.Pending,
            ExpiresAt = DateTime.UtcNow.AddDays(14),
            IsActive = true
        };

        await invitationRepo.AddAsync(invitation);

        deal.Status = DealStatus.PendingCounterparty;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Party Invited",
            $"A counterparty has been invited to deal {deal.Reference}.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Party invited successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> AcceptInvitationAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.PendingCounterparty)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in PendingCounterparty status to accept an invitation.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var party = await partyRepo.GetSingleByAsync(
            p => p.DealId == dealId && p.UserId == userId && !p.IsDeleted);

        if (party is not null)
        {
            party.Status = DealPartyStatus.Accepted;
            party.AcceptedAt = DateTime.UtcNow;
            await partyRepo.UpdateAsync(party);
        }

        deal.Status = DealStatus.TermsNegotiation;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            deal.CreatedByUserId, "Invitation Accepted",
            $"The invitation to deal {deal.Reference} has been accepted.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Invitation accepted successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> RejectInvitationAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.PendingCounterparty)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in PendingCounterparty status to reject an invitation.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var party = await partyRepo.GetSingleByAsync(
            p => p.DealId == dealId && p.UserId == userId && !p.IsDeleted);

        if (party is not null)
        {
            party.Status = DealPartyStatus.Rejected;
            await partyRepo.UpdateAsync(party);
        }

        deal.Status = DealStatus.Cancelled;
        deal.CancelledAt = DateTime.UtcNow;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            deal.CreatedByUserId, "Invitation Rejected",
            $"The invitation to deal {deal.Reference} has been rejected.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Invitation rejected successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> ProposeTermsAsync(Guid dealId, Guid userId, ProposeTermsRequest request, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.TermsNegotiation)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in TermsNegotiation status to propose terms.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        var agreementRepo = _unitOfWork.GetRepository<Agreement>();

        // Determine version by counting existing agreements for this deal
        var existingAgreements = await agreementRepo.GetAllDataAsync(a => a.DealId == dealId && !a.IsDeleted);
        var nextVersion = existingAgreements.Count() + 1;

        string? sectionsJson = null;
        if (request.Sections is not null && request.Sections.Count > 0)
        {
            sectionsJson = JsonConvert.SerializeObject(request.Sections);
        }

        var agreement = new Agreement
        {
            Id = Guid.NewGuid(),
            DealId = dealId,
            Version = nextVersion,
            GeneratedByAi = request.GeneratedByAi ?? false,
            SectionsJson = sectionsJson,
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

        deal.AgreementId = agreement.Id;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Terms Proposed",
            $"New terms have been proposed for deal {deal.Reference}.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Terms proposed successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> ApproveTermsAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.TermsNegotiation)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in TermsNegotiation status to approve terms.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        // Freeze the agreement if one exists
        if (deal.AgreementId.HasValue)
        {
            var agreementRepo = _unitOfWork.GetRepository<Agreement>();
            var agreement = await agreementRepo.GetByIdAsync(deal.AgreementId.Value);
            if (agreement is not null)
            {
                agreement.FrozenAt = DateTime.UtcNow;
                await agreementRepo.UpdateAsync(agreement);
            }
        }

        deal.Status = DealStatus.AwaitingFunding;
        deal.TermsAcceptedAt = DateTime.UtcNow;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Terms Approved",
            $"Terms have been approved for deal {deal.Reference}.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Terms approved successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> InitiateFundingAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");

        if (deal.Status != DealStatus.AwaitingFunding)
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in AwaitingFunding status to initiate funding.");

        // Only the Buyer party may fund
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var buyerParty = await partyRepo.GetSingleByAsync(
            p => p.DealId == dealId && p.UserId == userId && p.PartyType == PartyType.Buyer && !p.IsDeleted);

        if (buyerParty is null)
            return NaitrustResponse<DealResponse>.Forbidden("Only the Buyer party can fund this deal.");

        // Fetch buyer's Settlement subledger
        var vaRepo = _unitOfWork.GetRepository<VirtualAccount>();
        var buyerWallet = await vaRepo.GetSingleByAsync(
            va => va.UserId == userId && va.Type == VirtualAccountType.Settlement && !va.IsDeleted);

        if (buyerWallet is null || string.IsNullOrEmpty(buyerWallet.ProviderReference))
            return NaitrustResponse<DealResponse>.BadRequest(
                "Buyer does not have a wallet. Please complete KYC first.");

        // Fetch platform escrow subledger
        var platformEscrow = await vaRepo.GetSingleByAsync(
            va => va.Type == VirtualAccountType.Platform && !va.IsDeleted);

        if (platformEscrow is null || string.IsNullOrEmpty(platformEscrow.ProviderReference))
            return NaitrustResponse<DealResponse>.BadRequest(
                "Platform escrow account is not set up. Contact support.");

        // Verify buyer has sufficient balance
        var balanceResult = await _anchor.GetFundingStatusAsync(
            new FundingStatusRequest(buyerWallet.ProviderReference), ct);

        if (balanceResult.AmountReceivedMinor < deal.AmountMinor)
            return NaitrustResponse<DealResponse>.BadRequest(
                $"Insufficient wallet balance. Required: {deal.AmountMinor / 100m:N2} NGN, Available: {balanceResult.AmountReceivedMinor / 100m:N2} NGN.");

        // Internal transfer: buyer subledger → platform escrow
        var idempotencyKey = $"fund-{dealId}";
        var transfer = await _anchor.InternalTransferAsync(
            sourceSubAccountId: buyerWallet.ProviderReference,
            destinationSubAccountId: platformEscrow.ProviderReference,
            amountMinor: deal.AmountMinor,
            currency: deal.Currency,
            narration: $"Escrow funding for deal {deal.Reference}",
            idempotencyKey: idempotencyKey,
            ct: ct);

        // Record payment instruction
        var instrRepo = _unitOfWork.GetRepository<PaymentInstruction>();
        var instruction = new PaymentInstruction
        {
            Id = Guid.NewGuid(),
            TransactionId = dealId,
            VirtualAccountId = buyerWallet.Id,
            InstructionType = PaymentInstructionType.EscrowFunding,
            Partner = PaymentPartnerId.Anchor,
            IdempotencyKey = idempotencyKey,
            Status = PaymentInstructionStatus.Confirmed,
            PartnerReference = transfer.PartnerReference,
            IsActive = true
        };
        await instrRepo.AddAsync(instruction);

        // Double-entry ledger: debit escrow (funds received), credit buyer wallet (funds left)
        var ledgerRepo = _unitOfWork.GetRepository<LedgerEntry>();
        var entryGroupId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        await ledgerRepo.AddAsync(new LedgerEntry
        {
            Id = Guid.NewGuid(),
            TransactionId = dealId,
            EntryGroupId = entryGroupId,
            EventType = LedgerEventType.FundingConfirmed,
            Account = $"Escrow:{dealId}",
            DebitMinor = deal.AmountMinor,
            CreditMinor = 0,
            Currency = deal.Currency,
            Memo = $"Buyer escrow deposit – deal {deal.Reference}",
            CreatedAt = now
        });

        await ledgerRepo.AddAsync(new LedgerEntry
        {
            Id = Guid.NewGuid(),
            TransactionId = dealId,
            EntryGroupId = entryGroupId,
            EventType = LedgerEventType.FundingConfirmed,
            Account = $"Buyer:{userId}",
            DebitMinor = 0,
            CreditMinor = deal.AmountMinor,
            Currency = deal.Currency,
            Memo = $"Buyer wallet debit – deal {deal.Reference}",
            CreatedAt = now
        });

        // Advance deal state
        deal.Status = DealStatus.Funded;
        deal.PaymentStatus = PaymentStatus.PaymentConfirmedByPartner;
        deal.UpdatedAt = now;
        await dealRepo.UpdateAsync(deal);

        // Seed default tracking milestones (idempotent)
        await SeedDefaultMilestonesAsync(dealId, userId, now);

        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Deal Funded",
            $"Deal {deal.Reference} has been funded successfully. The funds are in escrow.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Deal funded successfully. Funds are held in escrow.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> SubmitDeliveryAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.Funded)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in Funded status to submit delivery.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        deal.Status = DealStatus.InProgress;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Delivery Submitted",
            $"Delivery has been submitted for deal {deal.Reference}.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Delivery submitted successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> ConfirmDeliveryAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");

        if (deal.Status != DealStatus.InProgress)
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in InProgress status to confirm delivery.");

        // Only the Buyer confirms delivery
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var buyerParty = await partyRepo.GetSingleByAsync(
            p => p.DealId == dealId && p.UserId == userId && p.PartyType == PartyType.Buyer && !p.IsDeleted);

        if (buyerParty is null)
            return NaitrustResponse<DealResponse>.Forbidden("Only the Buyer party can confirm delivery.");

        // ── Release funds from escrow to seller ──────────────────────────────
        if (deal.PaymentStatus == PaymentStatus.PaymentConfirmedByPartner)
        {
            // Find the seller party (non-buyer with a registered UserId)
            var allParties = await partyRepo.GetAllDataAsync(p => p.DealId == dealId && !p.IsDeleted);
            var sellerParty = allParties.FirstOrDefault(
                p => p.UserId.HasValue && p.UserId != userId);

            if (sellerParty?.UserId is null)
                return NaitrustResponse<DealResponse>.BadRequest(
                    "Cannot release funds: no registered seller party found on this deal.");

            var vaRepo = _unitOfWork.GetRepository<VirtualAccount>();

            var sellerWallet = await vaRepo.GetSingleByAsync(
                va => va.UserId == sellerParty.UserId && va.Type == VirtualAccountType.Settlement && !va.IsDeleted);

            if (sellerWallet is null || string.IsNullOrEmpty(sellerWallet.ProviderReference))
                return NaitrustResponse<DealResponse>.BadRequest(
                    "Cannot release funds: the seller has not completed KYC and does not have a wallet yet.");

            var platformEscrow = await vaRepo.GetSingleByAsync(
                va => va.Type == VirtualAccountType.Platform && !va.IsDeleted);

            if (platformEscrow is null || string.IsNullOrEmpty(platformEscrow.ProviderReference))
                return NaitrustResponse<DealResponse>.BadRequest(
                    "Platform escrow account is not set up. Contact support.");

            var idempotencyKey = $"release-{dealId}";
            var transfer = await _anchor.InternalTransferAsync(
                sourceSubAccountId: platformEscrow.ProviderReference,
                destinationSubAccountId: sellerWallet.ProviderReference,
                amountMinor: deal.AmountMinor,
                currency: deal.Currency,
                narration: $"Escrow release to seller – deal {deal.Reference}",
                idempotencyKey: idempotencyKey,
                ct: ct);

            var instrRepo = _unitOfWork.GetRepository<PaymentInstruction>();
            await instrRepo.AddAsync(new PaymentInstruction
            {
                Id = Guid.NewGuid(),
                TransactionId = dealId,
                VirtualAccountId = sellerWallet.Id,
                InstructionType = PaymentInstructionType.Release,
                Partner = PaymentPartnerId.Anchor,
                IdempotencyKey = idempotencyKey,
                Status = PaymentInstructionStatus.Confirmed,
                PartnerReference = transfer.PartnerReference,
                IsActive = true
            });

            var now = DateTime.UtcNow;
            var entryGroupId = Guid.NewGuid();
            var ledgerRepo = _unitOfWork.GetRepository<LedgerEntry>();

            // Debit escrow (funds leaving escrow), credit seller wallet (funds arriving)
            await ledgerRepo.AddAsync(new LedgerEntry
            {
                Id = Guid.NewGuid(),
                TransactionId = dealId,
                EntryGroupId = entryGroupId,
                EventType = LedgerEventType.SellerPayoutExecuted,
                Account = $"Escrow:{dealId}",
                DebitMinor = 0,
                CreditMinor = deal.AmountMinor,
                Currency = deal.Currency,
                Memo = $"Escrow release – deal {deal.Reference}",
                CreatedAt = now
            });

            await ledgerRepo.AddAsync(new LedgerEntry
            {
                Id = Guid.NewGuid(),
                TransactionId = dealId,
                EntryGroupId = entryGroupId,
                EventType = LedgerEventType.SellerPayoutExecuted,
                Account = $"Seller:{sellerParty.UserId}",
                DebitMinor = deal.AmountMinor,
                CreditMinor = 0,
                Currency = deal.Currency,
                Memo = $"Seller payout received – deal {deal.Reference}",
                CreatedAt = now
            });

            deal.PaymentStatus = PaymentStatus.Released;

            await _notificationService.SendNotificationAsync(
                sellerParty.UserId.Value, "Payment Released",
                $"Deal {deal.Reference} is complete. {deal.AmountMinor / 100m:N2} {deal.Currency} has been credited to your wallet.",
                "DealUpdate", null, ct);
        }

        var completedAt = DateTime.UtcNow;
        deal.Status = DealStatus.Completed;
        deal.CompletedAt = completedAt;
        deal.UpdatedAt = completedAt;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Deal Completed",
            $"Deal {deal.Reference} has been completed and funds released to the seller.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Delivery confirmed and funds released to seller.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> CancelDealAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");

        if (deal.Status == DealStatus.Cancelled || deal.Status == DealStatus.Completed)
            return NaitrustResponse<DealResponse>.BadRequest("Deal is already in a terminal status and cannot be cancelled.");

        if (!await IsUserPartyToDeal(dealId, userId))
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");

        // ── Refund escrow to buyer if deal was funded ─────────────────────────
        if (deal.PaymentStatus == PaymentStatus.PaymentConfirmedByPartner)
        {
            var partyRepo = _unitOfWork.GetRepository<DealParty>();
            var buyerParty = await partyRepo.GetSingleByAsync(
                p => p.DealId == dealId && p.PartyType == PartyType.Buyer && p.UserId.HasValue && !p.IsDeleted);

            if (buyerParty?.UserId is null)
                return NaitrustResponse<DealResponse>.BadRequest(
                    "Cannot refund: buyer party not found on this deal.");

            var vaRepo = _unitOfWork.GetRepository<VirtualAccount>();

            var buyerWallet = await vaRepo.GetSingleByAsync(
                va => va.UserId == buyerParty.UserId && va.Type == VirtualAccountType.Settlement && !va.IsDeleted);

            if (buyerWallet is null || string.IsNullOrEmpty(buyerWallet.ProviderReference))
                return NaitrustResponse<DealResponse>.BadRequest(
                    "Cannot refund: buyer wallet not found. Contact support.");

            var platformEscrow = await vaRepo.GetSingleByAsync(
                va => va.Type == VirtualAccountType.Platform && !va.IsDeleted);

            if (platformEscrow is null || string.IsNullOrEmpty(platformEscrow.ProviderReference))
                return NaitrustResponse<DealResponse>.BadRequest(
                    "Platform escrow account is not set up. Contact support.");

            var idempotencyKey = $"refund-{dealId}";
            var transfer = await _anchor.InternalTransferAsync(
                sourceSubAccountId: platformEscrow.ProviderReference,
                destinationSubAccountId: buyerWallet.ProviderReference,
                amountMinor: deal.AmountMinor,
                currency: deal.Currency,
                narration: $"Escrow refund to buyer – deal {deal.Reference}",
                idempotencyKey: idempotencyKey,
                ct: ct);

            var instrRepo = _unitOfWork.GetRepository<PaymentInstruction>();
            await instrRepo.AddAsync(new PaymentInstruction
            {
                Id = Guid.NewGuid(),
                TransactionId = dealId,
                VirtualAccountId = buyerWallet.Id,
                InstructionType = PaymentInstructionType.Refund,
                Partner = PaymentPartnerId.Anchor,
                IdempotencyKey = idempotencyKey,
                Status = PaymentInstructionStatus.Confirmed,
                PartnerReference = transfer.PartnerReference,
                IsActive = true
            });

            var now = DateTime.UtcNow;
            var entryGroupId = Guid.NewGuid();
            var ledgerRepo = _unitOfWork.GetRepository<LedgerEntry>();

            // Debit escrow (funds leaving), credit buyer wallet (funds returning)
            await ledgerRepo.AddAsync(new LedgerEntry
            {
                Id = Guid.NewGuid(),
                TransactionId = dealId,
                EntryGroupId = entryGroupId,
                EventType = LedgerEventType.BuyerRefundExecuted,
                Account = $"Escrow:{dealId}",
                DebitMinor = 0,
                CreditMinor = deal.AmountMinor,
                Currency = deal.Currency,
                Memo = $"Escrow refund – deal {deal.Reference}",
                CreatedAt = now
            });

            await ledgerRepo.AddAsync(new LedgerEntry
            {
                Id = Guid.NewGuid(),
                TransactionId = dealId,
                EntryGroupId = entryGroupId,
                EventType = LedgerEventType.BuyerRefundExecuted,
                Account = $"Buyer:{buyerParty.UserId}",
                DebitMinor = deal.AmountMinor,
                CreditMinor = 0,
                Currency = deal.Currency,
                Memo = $"Buyer refund received – deal {deal.Reference}",
                CreatedAt = now
            });

            deal.PaymentStatus = PaymentStatus.Refunded;

            await _notificationService.SendNotificationAsync(
                buyerParty.UserId.Value, "Refund Processed",
                $"Deal {deal.Reference} was cancelled. {deal.AmountMinor / 100m:N2} {deal.Currency} has been refunded to your wallet.",
                "DealUpdate", null, ct);
        }

        var cancelledAt = DateTime.UtcNow;
        deal.Status = DealStatus.Cancelled;
        deal.CancelledAt = cancelledAt;
        deal.UpdatedAt = cancelledAt;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Deal Cancelled",
            $"Deal {deal.Reference} has been cancelled.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            deal.PaymentStatus == PaymentStatus.Refunded
                ? "Deal cancelled and escrow refunded to buyer."
                : "Deal cancelled successfully.",
            await BuildDealResponse(deal));
    }

    private async Task<bool> IsUserPartyToDeal(Guid dealId, Guid userId)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var party = await partyRepo.GetSingleByAsync(
            p => p.DealId == dealId && p.UserId == userId && !p.IsDeleted);
        return party is not null;
    }

    private async Task<DealResponse> BuildDealResponse(Deal deal)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.DealId == deal.Id && !p.IsDeleted);
        var partyResponses = parties.Select(MapToPartyResponse).ToList();

        AgreementResponse? agreementResponse = null;
        if (deal.AgreementId.HasValue)
        {
            var agreementRepo = _unitOfWork.GetRepository<Agreement>();
            var agreement = await agreementRepo.GetByIdAsync(deal.AgreementId.Value);
            if (agreement is not null && !agreement.IsDeleted)
            {
                agreementResponse = MapToAgreementResponse(agreement);
            }
        }

        var allowedActions = GetAllowedActions(deal.Status);

        return MapToResponse(deal, partyResponses, agreementResponse, allowedActions);
    }

    private static DealResponse MapToResponse(
        Deal deal,
        List<DealPartyResponse>? parties,
        AgreementResponse? agreement,
        List<AllowedActionDto>? allowedActions = null)
    {
        return new DealResponse(
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
            parties,
            agreement,
            allowedActions,
            null,
            deal.CreatedAt);
    }

    private static DealPartyResponse MapToPartyResponse(DealParty party)
    {
        return new DealPartyResponse(
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
        var sections = DeserializeSections(agreement.SectionsJson);
        return new AgreementResponse(
            agreement.Id,
            agreement.Version,
            agreement.GeneratedByAi,
            sections);
    }

    private static List<AgreementSectionResponse> DeserializeSections(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<AgreementSectionResponse>();
        try
        {
            return JsonConvert.DeserializeObject<List<AgreementSectionResponse>>(json)
                ?? new List<AgreementSectionResponse>();
        }
        catch
        {
            return new List<AgreementSectionResponse>();
        }
    }

    private static List<AllowedActionDto> GetAllowedActions(DealStatus status)
    {
        return status switch
        {
            DealStatus.Draft => new List<AllowedActionDto>
            {
                new("InviteParty", "Invite Counterparty", true),
                new("Update", "Edit Deal", true),
                new("Cancel", "Cancel Deal", true)
            },
            DealStatus.PendingCounterparty => new List<AllowedActionDto>
            {
                new("AcceptInvitation", "Accept Invitation", true),
                new("RejectInvitation", "Reject Invitation", true),
                new("Cancel", "Cancel Deal", true)
            },
            DealStatus.TermsNegotiation => new List<AllowedActionDto>
            {
                new("ProposeTerms", "Propose Terms", true),
                new("ApproveTerms", "Approve Terms", true),
                new("Cancel", "Cancel Deal", true)
            },
            DealStatus.AwaitingFunding => new List<AllowedActionDto>
            {
                new("InitiateFunding", "Initiate Funding", true),
                new("Cancel", "Cancel Deal", true)
            },
            DealStatus.Funded => new List<AllowedActionDto>
            {
                new("SubmitDelivery", "Submit Delivery", true),
                new("Cancel", "Cancel Deal", true)
            },
            DealStatus.InProgress => new List<AllowedActionDto>
            {
                new("ConfirmDelivery", "Confirm Delivery", true),
                new("Cancel", "Cancel Deal", true)
            },
            DealStatus.Completed => new List<AllowedActionDto>(),
            DealStatus.Cancelled => new List<AllowedActionDto>(),
            _ => new List<AllowedActionDto>()
        };
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task SeedDefaultMilestonesAsync(Guid dealId, Guid fundedByUserId, DateTime now)
    {
        var repo = _unitOfWork.GetRepository<Milestone>();
        var existing = await repo.GetAllDataAsync(m => m.DealId == dealId && !m.IsDeleted);
        if (existing.Any()) { return; }

        // Mirror the UI's default MILESTONE_STAGES
        var stages = new[]
        {
            ("Order confirmed",       "Both parties agreed the terms and the deal is funded."),
            ("Dispatched",            "The seller picked up and dispatched the goods."),
            ("In transit",            "Goods are on the way to the destination."),
            ("Arrived",               "Goods reached the destination for handover."),
            ("Delivered & confirmed", "Buyer confirmed delivery — release can proceed.")
        };

        for (var i = 0; i < stages.Length; i++)
        {
            var (title, description) = stages[i];

            // First stage is already "done" at funding; second is "current"; rest are "pending"
            var status = i == 0
                ? MilestoneStatus.Approved
                : i == 1
                    ? MilestoneStatus.InProgress
                    : MilestoneStatus.Pending;

            await repo.AddAsync(new Milestone
            {
                Id = Guid.NewGuid(),
                DealId = dealId,
                Title = title,
                Description = description,
                SortOrder = i,
                Status = status,
                UpdatedByUserId = status == MilestoneStatus.Approved ? fundedByUserId : null,
                StatusChangedAt = status == MilestoneStatus.Approved ? now : null,
                IsActive = true
            });
        }
    }
}
