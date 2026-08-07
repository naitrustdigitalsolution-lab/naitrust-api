using Naitrust.Application.Services.Implementations.Invitations;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Transactions;

// The single authority for all deal state transitions
public class DealOrchestrator : IDealOrchestrator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public DealOrchestrator(IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
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
            sectionsJson = System.Text.Json.JsonSerializer.Serialize(request.Sections);
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
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.AwaitingFunding)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in AwaitingFunding status to initiate funding.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        // Stub: just transition the status
        deal.Status = DealStatus.Funded;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Funding Initiated",
            $"Funding has been initiated for deal {deal.Reference}.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Funding initiated successfully.",
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
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.InProgress)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal must be in InProgress status to confirm delivery.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        deal.Status = DealStatus.Completed;
        deal.CompletedAt = DateTime.UtcNow;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Delivery Confirmed",
            $"Delivery has been confirmed for deal {deal.Reference}. Deal completed.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Delivery confirmed successfully.",
            await BuildDealResponse(deal));
    }

    public async Task<NaitrustResponse<DealResponse>> CancelDealAsync(Guid dealId, Guid userId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        // Cannot cancel already terminal states
        if (deal.Status == DealStatus.Cancelled || deal.Status == DealStatus.Completed)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal is already in a terminal status and cannot be cancelled.");
        }

        if (!await IsUserPartyToDeal(dealId, userId))
        {
            return NaitrustResponse<DealResponse>.Forbidden("You are not a party to this deal.");
        }

        deal.Status = DealStatus.Cancelled;
        deal.CancelledAt = DateTime.UtcNow;
        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        await _notificationService.SendNotificationAsync(
            userId, "Deal Cancelled",
            $"Deal {deal.Reference} has been cancelled.",
            "DealUpdate", null, ct);

        return NaitrustResponse<DealResponse>.Success(
            "Deal cancelled successfully.",
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
            return System.Text.Json.JsonSerializer.Deserialize<List<AgreementSectionResponse>>(json,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true })
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
}
