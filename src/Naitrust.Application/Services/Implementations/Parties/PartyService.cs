using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Parties;

public class PartyService : IPartyService
{
    private readonly IUnitOfWork _unitOfWork;

    public PartyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<TransactionPartyResponse>> CreatePartyAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var transactionRepo = _unitOfWork.GetRepository<Transaction>();

        var transaction = await transactionRepo.GetByIdAsync(transactionId);
        if (transaction is null || transaction.IsDeleted)
        {
            return NaitrustResponse<TransactionPartyResponse>.NotFound("Transaction not found.");
        }

        // Attempt to look up user display name
        var displayName = string.Empty;
        var userRepo = _unitOfWork.GetRepository<NaitrustUser>();
        var user = await userRepo.GetSingleByAsync(u => u.Id == userId);
        if (user is not null)
        {
            displayName = $"{user.FirstName} {user.LastName}".Trim();
        }

        var party = new TransactionParty
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            UserId = userId,
            PartyType = PartyType.Buyer,
            PartyMode = transaction.PartyMode,
            DisplayName = displayName,
            Email = user?.Email,
            Status = TransactionPartyStatus.Invited,
            IsActive = true
        };

        await partyRepo.AddAsync(party);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<TransactionPartyResponse>.Created(
            "Party created successfully.",
            MapToPartyResponse(party));
    }

    public async Task<NaitrustResponse<TransactionPartyResponse>> GetPartyAsync(Guid partyId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var party = await partyRepo.GetByIdAsync(partyId);

        if (party is null || party.IsDeleted)
        {
            return NaitrustResponse<TransactionPartyResponse>.NotFound("Party not found.");
        }

        return NaitrustResponse<TransactionPartyResponse>.Success(
            "Party retrieved successfully.",
            MapToPartyResponse(party));
    }

    public async Task<NaitrustResponse<List<TransactionPartyResponse>>> GetPartiesByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var parties = await partyRepo.GetAllDataAsync(p => p.TransactionId == transactionId && !p.IsDeleted);

        var responses = parties.Select(MapToPartyResponse).ToList();

        return NaitrustResponse<List<TransactionPartyResponse>>.Success(
            "Parties retrieved successfully.",
            responses);
    }

    public async Task<NaitrustResponse<TransactionPartyResponse>> ResolvePartyAsync(Guid partyId, Guid userId, CancellationToken ct = default)
    {
        var partyRepo = _unitOfWork.GetRepository<TransactionParty>();
        var party = await partyRepo.GetByIdAsync(partyId);

        if (party is null || party.IsDeleted)
        {
            return NaitrustResponse<TransactionPartyResponse>.NotFound("Party not found.");
        }

        party.UserId = userId;
        party.Status = TransactionPartyStatus.Accepted;
        party.AcceptedAt = DateTime.UtcNow;
        party.UpdatedAt = DateTime.UtcNow;

        // Update display name from user if available
        var userRepo = _unitOfWork.GetRepository<NaitrustUser>();
        var user = await userRepo.GetSingleByAsync(u => u.Id == userId);
        if (user is not null)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            if (!string.IsNullOrEmpty(fullName))
            {
                party.DisplayName = fullName;
            }
        }

        await partyRepo.UpdateAsync(party);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<TransactionPartyResponse>.Success(
            "Party resolved successfully.",
            MapToPartyResponse(party));
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
}
