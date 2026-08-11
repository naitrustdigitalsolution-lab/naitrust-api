using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Negotiations;
using Naitrust.Domain.Models.Dtos.Responses.Negotiations;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Negotiations;

public class NegotiationService : INegotiationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public NegotiationService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<NegotiationResponse?>> GetByTransactionAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Negotiation>();
        var negotiation = await repo.GetSingleByAsync(n => n.DealId == transactionId && !n.IsDeleted);

        if (negotiation is null)
        {
            return NaitrustResponse<NegotiationResponse?>.Success("No negotiation found.", null);
        }

        var proposals = await GetProposalsAsync(negotiation.Id);
        return NaitrustResponse<NegotiationResponse?>.Success("Negotiation retrieved.", await MapToResponse(negotiation, proposals, userId));
    }

    public async Task<NaitrustResponse<NegotiationResponse>> ProposeAsync(Guid transactionId, Guid userId, ProposeNegotiationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Negotiation>();
        var proposalRepo = _unitOfWork.GetRepository<NegotiationProposal>();

        var existing = await repo.GetSingleByAsync(n => n.DealId == transactionId && !n.IsDeleted
            && (n.Status == NegotiationStatus.Open || n.Status == NegotiationStatus.CounterProposed));

        if (existing is not null)
        {
            // Supersede the latest pending proposal
            if (existing.LatestProposalId.HasValue)
            {
                var latestProposal = await proposalRepo.GetByIdAsync(existing.LatestProposalId.Value);
                if (latestProposal is not null && latestProposal.Status == ProposalStatus.Pending)
                {
                    latestProposal.Status = ProposalStatus.Superseded;
                    await proposalRepo.UpdateAsync(latestProposal);
                }
            }

            var counterProposal = new NegotiationProposal
            {
                Id = Guid.NewGuid(),
                NegotiationId = existing.Id,
                ProposedByUserId = userId,
                ProposedChangesJson = JsonConvert.SerializeObject(request.Changes),
                Message = request.Message,
                Status = ProposalStatus.Pending,
                IsActive = true
            };

            await proposalRepo.AddAsync(counterProposal);
            existing.LatestProposalId = counterProposal.Id;
            existing.Status = NegotiationStatus.Open;
            await repo.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            var allProposals = await GetProposalsAsync(existing.Id);
            return NaitrustResponse<NegotiationResponse>.Created("Counter-proposal submitted.", await MapToResponse(existing, allProposals, userId));
        }

        // Create new negotiation
        var negotiation = new Negotiation
        {
            Id = Guid.NewGuid(),
            DealId = transactionId,
            InitiatedByUserId = userId,
            Status = NegotiationStatus.Open,
            IsActive = true
        };

        await repo.AddAsync(negotiation);

        var proposal = new NegotiationProposal
        {
            Id = Guid.NewGuid(),
            NegotiationId = negotiation.Id,
            ProposedByUserId = userId,
            ProposedChangesJson = JsonConvert.SerializeObject(request.Changes),
            Message = request.Message,
            Status = ProposalStatus.Pending,
            IsActive = true
        };

        await proposalRepo.AddAsync(proposal);
        negotiation.LatestProposalId = proposal.Id;
        await repo.UpdateAsync(negotiation);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<NegotiationResponse>.Created("Negotiation started.", await MapToResponse(negotiation, new List<NegotiationProposal> { proposal }, userId));
    }

    public async Task<NaitrustResponse<NegotiationResponse>> RespondToProposalAsync(Guid transactionId, Guid proposalId, Guid userId, RespondToProposalRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Negotiation>();
        var proposalRepo = _unitOfWork.GetRepository<NegotiationProposal>();

        var negotiation = await repo.GetSingleByAsync(n => n.DealId == transactionId && !n.IsDeleted
            && (n.Status == NegotiationStatus.Open || n.Status == NegotiationStatus.CounterProposed));

        if (negotiation is null)
        {
            return NaitrustResponse<NegotiationResponse>.NotFound("No active negotiation found.");
        }

        var proposal = await proposalRepo.GetByIdAsync(proposalId);
        if (proposal is null || proposal.NegotiationId != negotiation.Id)
        {
            return NaitrustResponse<NegotiationResponse>.NotFound("Proposal not found.");
        }

        var action = request.Action.ToLowerInvariant();
        switch (action)
        {
            case "accepted":
                proposal.Status = ProposalStatus.Accepted;
                proposal.RespondedAt = DateTime.UtcNow;
                negotiation.Status = NegotiationStatus.Accepted;
                negotiation.ResolvedAt = DateTime.UtcNow;
                break;

            case "declined":
                proposal.Status = ProposalStatus.Rejected;
                proposal.RespondedAt = DateTime.UtcNow;
                negotiation.Status = NegotiationStatus.Rejected;
                negotiation.ResolvedAt = DateTime.UtcNow;
                break;

            default:
                return NaitrustResponse<NegotiationResponse>.BadRequest($"Invalid action: {request.Action}. Use 'accepted' or 'declined'.");
        }

        await proposalRepo.UpdateAsync(proposal);
        await repo.UpdateAsync(negotiation);
        await _unitOfWork.SaveChangesAsync();

        var allProposals = await GetProposalsAsync(negotiation.Id);
        return NaitrustResponse<NegotiationResponse>.Success("Proposal response recorded.", await MapToResponse(negotiation, allProposals, userId));
    }

    public async Task<NaitrustResponse<NegotiationResponse?>> WithdrawAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Negotiation>();
        var negotiation = await repo.GetSingleByAsync(n => n.DealId == transactionId && !n.IsDeleted
            && (n.Status == NegotiationStatus.Open || n.Status == NegotiationStatus.CounterProposed));

        if (negotiation is null)
        {
            return NaitrustResponse<NegotiationResponse?>.Success("No active negotiation to withdraw.", null);
        }

        negotiation.Status = NegotiationStatus.Expired; // Using Expired as "withdrawn"
        negotiation.ResolvedAt = DateTime.UtcNow;
        await repo.UpdateAsync(negotiation);
        await _unitOfWork.SaveChangesAsync();

        var proposals = await GetProposalsAsync(negotiation.Id);
        return NaitrustResponse<NegotiationResponse?>.Success("Negotiation withdrawn.", await MapToResponse(negotiation, proposals, userId));
    }

    private async Task<List<NegotiationProposal>> GetProposalsAsync(Guid negotiationId)
    {
        var proposalRepo = _unitOfWork.GetRepository<NegotiationProposal>();
        var proposals = await proposalRepo.GetAllDataAsync(p => p.NegotiationId == negotiationId && !p.IsDeleted);
        return proposals.OrderBy(p => p.CreatedAt).ToList();
    }

    private async Task<NegotiationResponse> MapToResponse(Negotiation negotiation, List<NegotiationProposal> proposals, Guid currentUserId)
    {
        // Frontend expects status: 'open' | 'accepted' | 'declined' | 'withdrawn'
        var status = negotiation.Status switch
        {
            NegotiationStatus.Open => "open",
            NegotiationStatus.CounterProposed => "open",
            NegotiationStatus.Accepted => "accepted",
            NegotiationStatus.Rejected => "declined",
            NegotiationStatus.Expired => "withdrawn",
            _ => negotiation.Status.ToString().ToLowerInvariant()
        };

        var proposalResponses = new List<NegotiationProposalResponse>();
        foreach (var p in proposals)
        {
            var user = await _userManager.FindByIdAsync(p.ProposedByUserId.ToString());
            var byName = p.ProposedByUserId == currentUserId ? "You"
                : user is not null ? $"{user.FirstName} {user.LastName}".Trim()
                : "Unknown";

            ProposedChangesResponse? changes = null;
            if (!string.IsNullOrEmpty(p.ProposedChangesJson))
            {
                var input = JsonConvert.DeserializeObject<ProposedChangesInput>(p.ProposedChangesJson);
                if (input is not null)
                {
                    changes = new ProposedChangesResponse(input.AmountMinor, input.DeliveryDueDate, input.ReleaseConditions, input.AgreementNote);
                }
            }
            changes ??= new ProposedChangesResponse(null, null, null, null);

            // Frontend ProposalStatus: 'proposed' | 'accepted' | 'declined' | 'superseded'
            var proposalStatus = p.Status switch
            {
                ProposalStatus.Pending => "proposed",
                ProposalStatus.Accepted => "accepted",
                ProposalStatus.Rejected => "declined",
                ProposalStatus.Superseded => "superseded",
                _ => p.Status.ToString().ToLowerInvariant()
            };

            proposalResponses.Add(new NegotiationProposalResponse(
                p.Id,
                byName,
                p.ProposedByUserId == currentUserId,
                p.Message ?? "",
                changes,
                proposalStatus,
                p.CreatedAt));
        }

        return new NegotiationResponse(negotiation.DealId, status, proposalResponses);
    }
}
