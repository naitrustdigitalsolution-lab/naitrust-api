using System.Security.Cryptography;
using Newtonsoft.Json;
using Naitrust.Application.ExternalServices.Communication;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Invitations;
using Naitrust.Domain.Models.Dtos.Responses.Invitations;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Invitations;

public class InvitationService : IInvitationService
{
    private readonly IUnitOfWork _unitOfWork;

    public InvitationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<List<InvitationListItemResponse>>> ListAsync(Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealInvitation>();
        var invitations = await repo.GetAllDataAsync(i =>
            !i.IsDeleted && (i.RecipientUserId == userId || i.InviterProfileId == userId));

        var items = invitations.Select(i => new InvitationListItemResponse(
            i.Id,
            i.DealId,
            i.FromName,
            i.YourRole,
            i.Title,
            i.AmountMinor,
            i.Currency,
            i.Status.ToString(),
            i.ExpiresAt,
            i.CreatedAt)).ToList();

        return NaitrustResponse<List<InvitationListItemResponse>>.Success("Invitations retrieved.", items);
    }

    public async Task<NaitrustResponse<DealInvitationResponse>> GetAsync(Guid invitationId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealInvitation>();
        var invitation = await repo.GetByIdAsync(invitationId);

        if (invitation is null || invitation.IsDeleted)
        {
            return NaitrustResponse<DealInvitationResponse>.NotFound("Invitation not found.");
        }

        var response = await MapToResponseAsync(invitation);
        return NaitrustResponse<DealInvitationResponse>.Success("Invitation retrieved.", response);
    }

    public async Task<NaitrustResponse<PublicInvitationPreviewResponse>> GetPublicPreviewAsync(string token, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealInvitation>();
        var invitation = await repo.GetSingleByAsync(i => i.PublicToken == token && !i.IsDeleted);

        if (invitation is null)
        {
            return NaitrustResponse<PublicInvitationPreviewResponse>.NotFound("Invitation not found.");
        }

        // Get transaction reference
        string? reference = null;
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(invitation.DealId);
        if (deal is not null)
        {
            reference = deal.Reference;
        }

        // Get inviter verification status
        bool inviterVerified = false;
        string? inviterAccountType = null;
        if (invitation.InviterProfileId.HasValue)
        {
            var businessRepo = _unitOfWork.GetRepository<Business>();
            var business = await businessRepo.GetByIdAsync(invitation.InviterProfileId.Value);
            if (business is not null)
            {
                inviterVerified = business.VerificationStatus == BusinessVerificationStatus.Verified;
                inviterAccountType = "business";
            }
            else
            {
                inviterAccountType = "personal";
            }
        }

        // Mask contact
        string? maskedContact = null;
        if (!string.IsNullOrEmpty(invitation.IntendedContact))
        {
            maskedContact = invitation.IntendedContact.Contains('@')
                ? EmailTemplateHelper.MaskEmail(invitation.IntendedContact)
                : MaskPhone(invitation.IntendedContact);
        }

        var preview = new PublicInvitationPreviewResponse(
            invitation.PublicToken,
            invitation.Id,
            reference,
            invitation.FromName,
            invitation.FromRole,
            invitation.YourRole,
            invitation.PartyMode,
            invitation.Title,
            invitation.AmountMinor,
            invitation.Currency,
            invitation.Message,
            inviterVerified,
            inviterAccountType,
            invitation.IntendedAccountType,
            maskedContact,
            invitation.ExpiresAt,
            invitation.Status.ToString());

        return NaitrustResponse<PublicInvitationPreviewResponse>.Success("Invitation preview retrieved.", preview);
    }

    public async Task<NaitrustResponse<DealInvitationResponse>> ClaimAsync(string token, ClaimInvitationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealInvitation>();
        var invitation = await repo.GetSingleByAsync(i => i.PublicToken == token && !i.IsDeleted);

        if (invitation is null)
        {
            return NaitrustResponse<DealInvitationResponse>.NotFound("Invitation not found.");
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            return NaitrustResponse<DealInvitationResponse>.BadRequest($"Invitation is already {invitation.Status}.");
        }

        if (invitation.ExpiresAt < DateTime.UtcNow)
        {
            invitation.Status = InvitationStatus.Expired;
            await repo.UpdateAsync(invitation);
            await _unitOfWork.SaveChangesAsync();
            return NaitrustResponse<DealInvitationResponse>.BadRequest("Invitation has expired.");
        }

        if (invitation.RecipientUserId.HasValue && invitation.RecipientUserId != request.UserId)
        {
            return NaitrustResponse<DealInvitationResponse>.BadRequest("This invitation was intended for a different user.");
        }

        invitation.RecipientUserId = request.UserId;
        invitation.InviteeProfileId = request.UserId;
        invitation.Status = InvitationStatus.Accepted;
        await repo.UpdateAsync(invitation);
        await _unitOfWork.SaveChangesAsync();

        var response = await MapToResponseAsync(invitation);
        return NaitrustResponse<DealInvitationResponse>.Success("Invitation claimed successfully.", response);
    }

    public async Task<NaitrustResponse<DealInvitationResponse>> RespondAsync(Guid invitationId, Guid userId, string action, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<DealInvitation>();
        var invitation = await repo.GetByIdAsync(invitationId);

        if (invitation is null || invitation.IsDeleted)
        {
            return NaitrustResponse<DealInvitationResponse>.NotFound("Invitation not found.");
        }

        if (invitation.Status != InvitationStatus.Pending && invitation.Status != InvitationStatus.Accepted)
        {
            return NaitrustResponse<DealInvitationResponse>.BadRequest($"Cannot respond to invitation in {invitation.Status} status.");
        }

        invitation.Status = action.ToLowerInvariant() switch
        {
            "accepted" => InvitationStatus.Accepted,
            "declined" => InvitationStatus.Declined,
            _ => invitation.Status
        };

        if (invitation.RecipientUserId is null)
        {
            invitation.RecipientUserId = userId;
        }

        await repo.UpdateAsync(invitation);
        await _unitOfWork.SaveChangesAsync();

        var response = await MapToResponseAsync(invitation);
        return NaitrustResponse<DealInvitationResponse>.Success($"Invitation {action}.", response);
    }

    public static string GeneratePublicToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(24);
        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

    private async Task<DealInvitationResponse> MapToResponseAsync(DealInvitation i)
    {
        // Get transaction reference
        string? reference = null;
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(i.DealId);
        if (deal is not null)
        {
            reference = deal.Reference;
        }

        // Parse AgreementSnapshot JSON into structured DTO
        AgreementSnapshotDto? agreement = null;
        if (!string.IsNullOrEmpty(i.AgreementSnapshot))
        {
            try
            {
                var sections = JsonConvert.DeserializeObject<List<AgreementSectionResponse>>(i.AgreementSnapshot);
                if (sections is not null)
                {
                    agreement = new AgreementSnapshotDto(sections);
                }
            }
            catch
            {
                // Try as wrapper object
                try
                {
                    agreement = JsonConvert.DeserializeObject<AgreementSnapshotDto>(i.AgreementSnapshot);
                }
                catch
                {
                    // If it's not valid JSON, ignore
                }
            }
        }

        return new DealInvitationResponse(
            i.Id,
            i.DealId,
            i.PublicToken,
            i.RecipientUserId,
            i.IntendedContact,
            i.IntendedAccountType,
            i.InviterProfileId,
            i.InviteeProfileId,
            i.PostAuthDestination,
            i.FromName,
            i.FromRole,
            i.YourRole,
            i.PartyMode,
            i.Title,
            i.AmountMinor,
            i.Currency,
            i.Message,
            reference,
            agreement,
            i.Status.ToString(),
            i.ExpiresAt,
            i.CreatedAt);
    }

    private static string MaskPhone(string phone)
    {
        if (phone.Length <= 4) { return "****"; }
        return new string('*', phone.Length - 4) + phone[^4..];
    }
}
