using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Implementations.Invitations;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Transactions;

public class DealService : IDealService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public DealService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<DealResponse>> CreateDealAsync(Guid userId, CreateDealRequest request, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var partyRepo = _unitOfWork.GetRepository<DealParty>();

        if (!Enum.TryParse<DealType>(request.DealType, ignoreCase: true, out var dealType))
        {
            return NaitrustResponse<DealResponse>.BadRequest($"Invalid deal type: {request.DealType}");
        }

        var creatorRole = request.Role?.ToLowerInvariant() == "seller" ? PartyType.Seller : PartyType.Buyer;

        // Use explicit partyMode from request if provided, otherwise infer
        PartyMode partyMode;
        if (!string.IsNullOrWhiteSpace(request.PartyMode) && Enum.TryParse<PartyMode>(request.PartyMode, ignoreCase: true, out var pm))
        {
            partyMode = pm;
        }
        else
        {
            partyMode = (request.Participants?.Any(p => p.Email is not null) ?? false) ? PartyMode.B2C : PartyMode.B2B;
        }

        var reference = $"NTR-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        var user = await _userManager.FindByIdAsync(userId.ToString());
        var creatorName = user is not null ? $"{user.FirstName} {user.LastName}".Trim() : string.Empty;

        var deal = new Deal
        {
            Id = Guid.NewGuid(),
            Reference = reference,
            UseCase = request.UseCase,
            DealType = dealType,
            CreatedByUserId = userId,
            PartyMode = partyMode,
            Title = request.Title,
            Description = request.Description,
            Category = DealCategory.Other,
            AmountMinor = request.AmountMinor,
            FeeMinor = 0,
            Currency = request.Currency,
            DeliveryDueDate = request.DeliveryDueDate,
            ReleaseConditions = request.ReleaseConditions,
            ExtendedProductTestingDays = request.ExtendedProductTestingDays,
            ExpiresAt = request.ExpiresInDays.HasValue ? DateTime.UtcNow.AddDays(request.ExpiresInDays.Value) : null,
            Recurring = dealType == DealType.Recurring,
            Status = DealStatus.Draft,
            PaymentStatus = PaymentStatus.NotStarted,
            IsActive = true
        };

        await dealRepo.AddAsync(deal);

        // Create the creator as a party
        var creatorParty = new DealParty
        {
            Id = Guid.NewGuid(),
            DealId = deal.Id,
            UserId = userId,
            PartyType = creatorRole,
            PartyMode = partyMode,
            DisplayName = creatorName,
            Status = DealPartyStatus.Accepted,
            AcceptedAt = DateTime.UtcNow,
            IsActive = true
        };

        await partyRepo.AddAsync(creatorParty);

        // Create participants as DealParties
        string? publicInvitePath = null;
        if (request.Participants is not null)
        {
            var invitationRepo = _unitOfWork.GetRepository<DealInvitation>();
            var counterRole = creatorRole == PartyType.Buyer ? PartyType.Seller : PartyType.Buyer;

            foreach (var participant in request.Participants)
            {
                // If profileId is provided, try to link to an existing user
                Guid? linkedUserId = null;
                if (!string.IsNullOrWhiteSpace(participant.ProfileId) && Guid.TryParse(participant.ProfileId, out var pid))
                {
                    linkedUserId = pid;
                }

                var party = new DealParty
                {
                    Id = Guid.NewGuid(),
                    DealId = deal.Id,
                    UserId = linkedUserId,
                    PartyType = counterRole,
                    PartyMode = partyMode,
                    DisplayName = participant.Name,
                    Email = participant.Email,
                    Phone = participant.Phone,
                    AllocationMinor = participant.AllocationMinor,
                    Status = DealPartyStatus.Invited,
                    IsActive = true
                };

                await partyRepo.AddAsync(party);

                // Auto-create DealInvitation for participants with email or phone
                var contact = !string.IsNullOrWhiteSpace(participant.Email) ? participant.Email
                    : !string.IsNullOrWhiteSpace(participant.Phone) ? participant.Phone
                    : !string.IsNullOrWhiteSpace(participant.Identifier) ? participant.Identifier
                    : null;

                if (contact is not null)
                {
                    var token = InvitationService.GeneratePublicToken();

                    var invitation = new DealInvitation
                    {
                        Id = Guid.NewGuid(),
                        DealId = deal.Id,
                        PublicToken = token,
                        IntendedContact = contact,
                        InviterProfileId = userId,
                        FromName = creatorName,
                        FromRole = creatorRole.ToString().ToLowerInvariant(),
                        YourRole = counterRole.ToString().ToLowerInvariant(),
                        PartyMode = partyMode.ToString(),
                        Title = deal.Title,
                        AmountMinor = deal.AmountMinor,
                        Currency = deal.Currency,
                        Status = InvitationStatus.Pending,
                        ExpiresAt = deal.ExpiresAt ?? DateTime.UtcNow.AddDays(14),
                        IsActive = true
                    };

                    await invitationRepo.AddAsync(invitation);

                    publicInvitePath ??= $"/invitations/public/{token}";
                }
            }

            if (request.Participants.Count > 0)
            {
                deal.Status = DealStatus.PendingCounterparty;
            }
        }

        // Create Agreement if provided
        if (request.Agreement is not null)
        {
            var agreementRepo = _unitOfWork.GetRepository<Agreement>();
            string? sectionsJson = null;
            if (request.Agreement.Sections is not null && request.Agreement.Sections.Count > 0)
            {
                sectionsJson = JsonSerializer.Serialize(request.Agreement.Sections);
            }

            var agreement = new Agreement
            {
                Id = Guid.NewGuid(),
                DealId = deal.Id,
                Version = request.Agreement.Version,
                GeneratedByAi = request.Agreement.GeneratedByAi,
                SectionsJson = sectionsJson,
                CreatedByUserId = userId,
                IsActive = true
            };

            await agreementRepo.AddAsync(agreement);
            deal.AgreementId = agreement.Id;
        }

        await _unitOfWork.SaveChangesAsync();

        var parties = new List<DealPartyResponse> { MapToPartyResponse(creatorParty, userId) };

        return NaitrustResponse<DealResponse>.Created(
            "Deal created successfully.",
            MapToResponse(deal, parties, null, null, publicInvitePath));
    }

    public async Task<NaitrustResponse<DealResponse>> GetDealAsync(Guid dealId, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.DealId == dealId && !p.IsDeleted);
        var partyResponses = parties.Select(p => MapToPartyResponse(p)).ToList();

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

        return NaitrustResponse<DealResponse>.Success(
            "Deal retrieved successfully.",
            MapToResponse(deal, partyResponses, agreementResponse, allowedActions));
    }

    public async Task<NaitrustResponse<PaginatedResponse<DealResponse>>> ListDealsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var dealRepo = _unitOfWork.GetRepository<Deal>();

        var userParties = await partyRepo.GetAllDataAsync(p => p.UserId == userId && !p.IsDeleted);
        var dealIds = userParties.Select(p => p.DealId).Distinct().ToList();

        if (!dealIds.Any())
        {
            return NaitrustResponse<PaginatedResponse<DealResponse>>.Success(
                "Deals retrieved successfully.",
                new PaginatedResponse<DealResponse>(new List<DealResponse>(), pagination.Page, pagination.PageSize, 0, 0));
        }

        var allDeals = await dealRepo.GetAllDataAsync(t => dealIds.Contains(t.Id) && !t.IsDeleted);
        var totalCount = allDeals.Count();

        var pagedDeals = allDeals
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var dealResponses = new List<DealResponse>();
        foreach (var deal in pagedDeals)
        {
            var parties = await partyRepo.GetAllDataAsync(p => p.DealId == deal.Id && !p.IsDeleted);
            var partyResponses = parties.Select(p => MapToPartyResponse(p, userId)).ToList();
            var allowedActions = GetAllowedActions(deal.Status);
            dealResponses.Add(MapToResponse(deal, partyResponses, null, allowedActions));
        }

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<DealResponse>>.Success(
            "Deals retrieved successfully.",
            new PaginatedResponse<DealResponse>(dealResponses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }

    public async Task<NaitrustResponse<DealResponse>> UpdateDealAsync(Guid dealId, UpdateDealRequest request, CancellationToken ct = default)
    {
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(dealId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<DealResponse>.NotFound("Deal not found.");
        }

        if (deal.Status != DealStatus.Draft)
        {
            return NaitrustResponse<DealResponse>.BadRequest("Deal can only be updated while in Draft status.");
        }

        if (request.Title is not null)
        {
            deal.Title = request.Title;
        }

        if (request.Description is not null)
        {
            deal.Description = request.Description;
        }

        if (request.AmountMinor.HasValue)
        {
            deal.AmountMinor = request.AmountMinor.Value;
        }

        deal.UpdatedAt = DateTime.UtcNow;
        await dealRepo.UpdateAsync(deal);
        await _unitOfWork.SaveChangesAsync();

        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.DealId == dealId && !p.IsDeleted);
        var partyResponses = parties.Select(p => MapToPartyResponse(p)).ToList();
        var allowedActions = GetAllowedActions(deal.Status);

        return NaitrustResponse<DealResponse>.Success(
            "Deal updated successfully.",
            MapToResponse(deal, partyResponses, null, allowedActions));
    }

    public async Task<NaitrustResponse<List<DealTypeResponse>>> GetDealTypesAsync(CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<TransactionType>();
        var types = await repo.GetAllDataAsync(t => t.IsActive && !t.IsDeleted);

        var responses = types.Select(t => new DealTypeResponse(
            t.Id,
            t.Key,
            t.Name,
            t.RequiredVerificationLevel.ToString(),
            t.ReleaseMode.ToString(),
            t.AutoConfirmWindowHours,
            t.IsActive)).ToList();

        return NaitrustResponse<List<DealTypeResponse>>.Success(
            "Deal types retrieved successfully.", responses);
    }

    private static DealResponse MapToResponse(
        Deal deal,
        List<DealPartyResponse>? parties,
        AgreementResponse? agreement,
        List<AllowedActionDto>? allowedActions = null,
        string? publicInvitePath = null)
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
            publicInvitePath,
            deal.CreatedAt);
    }

    private static DealPartyResponse MapToPartyResponse(DealParty party, Guid? currentUserId = null)
    {
        return new DealPartyResponse(
            party.Id,
            party.UserId,
            party.BusinessId,
            party.PartyType.ToString(),
            party.DisplayName,
            party.Email,
            party.Status.ToString(),
            party.AcceptedAt,
            IsYou: currentUserId.HasValue && party.UserId == currentUserId);
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
            return JsonSerializer.Deserialize<List<AgreementSectionResponse>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
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
