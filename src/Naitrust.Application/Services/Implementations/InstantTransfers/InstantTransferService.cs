using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.InstantTransfers;
using Naitrust.Domain.Models.Dtos.Responses.InstantTransfers;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.InstantTransfers;

public class InstantTransferService : IInstantTransferService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<NaitrustUser> _userManager;

    public InstantTransferService(IUnitOfWork unitOfWork, UserManager<NaitrustUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<NaitrustResponse<ValidateRecipientResponse>> ValidateRecipientAsync(
        Guid userId, ValidateRecipientRequest request, CancellationToken ct = default)
    {
        // Resolve the display name based on method
        string resolvedName = await ResolveNameAsync(request.Method, request.Identifier,
            request.NaitrustAccountNumber, request.NaitrustId);

        if (string.IsNullOrEmpty(resolvedName))
            return NaitrustResponse<ValidateRecipientResponse>.NotFound(
                "Recipient could not be found with the provided details.");

        var response = new ValidateRecipientResponse(
            request.Method,
            request.Identifier,
            resolvedName,
            request.BankName,
            request.NaitrustAccountNumber,
            request.NaitrustId,
            request.AccountType,
            IdentityVerified: false);

        return NaitrustResponse<ValidateRecipientResponse>.Success("Recipient validated.", response);
    }

    public async Task<NaitrustResponse<InstantTransferResponse>> CreateAsync(
        Guid userId, CreateInstantTransferRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<InstantTransfer>();

        var transfer = new InstantTransfer
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Reference = GenerateReference(),
            RecipientMethod = request.Recipient.Method,
            RecipientIdentifier = request.Recipient.Identifier,
            RecipientResolvedName = request.Recipient.ResolvedName,
            RecipientBankName = request.Recipient.BankName,
            RecipientNaitrustAccountNumber = request.Recipient.NaitrustAccountNumber,
            RecipientNaitrustId = request.Recipient.NaitrustId,
            RecipientAccountType = request.Recipient.AccountType,
            RecipientIdentityVerified = request.Recipient.IdentityVerified,
            AmountMinor = request.AmountMinor,
            Currency = request.Currency,
            FeeMinor = 0,
            Narration = request.Narration,
            Status = "processing",
            Provider = "mock",
            IsMock = true,
            IsActive = true,
        };

        await repo.AddAsync(transfer);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<InstantTransferResponse>.Success("Transfer initiated.", MapToResponse(transfer));
    }

    public async Task<NaitrustResponse<InstantTransferResponse>> GetByIdAsync(
        Guid userId, Guid transferId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<InstantTransfer>();
        var transfer = await repo.GetByIdAsync(transferId);

        if (transfer is null || transfer.IsDeleted || transfer.UserId != userId)
            return NaitrustResponse<InstantTransferResponse>.NotFound("Transfer not found.");

        return NaitrustResponse<InstantTransferResponse>.Success("Transfer retrieved.", MapToResponse(transfer));
    }

    public async Task<NaitrustResponse<List<InstantTransferResponse>>> GetMyAsync(
        Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<InstantTransfer>();
        var transfers = await repo.GetAllDataAsync(
            t => t.UserId == userId && !t.IsDeleted,
            orderBy: q => q.OrderByDescending(t => t.CreatedAt));

        return NaitrustResponse<List<InstantTransferResponse>>.Success(
            "Transfers retrieved.",
            transfers.Select(MapToResponse).ToList());
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task<string> ResolveNameAsync(
        string method, string identifier,
        string? naitrustAccountNumber, string? naitrustId)
    {
        switch (method)
        {
            case "email_address":
                var byEmail = await _userManager.FindByEmailAsync(identifier);
                return byEmail is not null
                    ? $"{byEmail.FirstName} {byEmail.LastName}".Trim()
                    : string.Empty;

            case "phone_number":
                var byPhone = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == identifier);
                return byPhone is not null
                    ? $"{byPhone.FirstName} {byPhone.LastName}".Trim()
                    : string.Empty;

            case "naitrust_account_number":
            case "naitrust_id":
                var vaRepo = _unitOfWork.GetRepository<VirtualAccount>();
                VirtualAccount? va = null;
                var lookupKey = !string.IsNullOrEmpty(naitrustAccountNumber) ? naitrustAccountNumber : identifier;
                va = await vaRepo.GetSingleByAsync(v => v.AccountNumber == lookupKey && !v.IsDeleted);

                if (va is not null)
                {
                    var owner = await _userManager.FindByIdAsync(va.UserId.ToString()!);
                    return owner is not null ? $"{owner.FirstName} {owner.LastName}".Trim() : string.Empty;
                }
                return string.Empty;

            case "bank_transfer":
                return "Bank Account Holder";

            default:
                return string.Empty;
        }
    }

    private static string GenerateReference()
        => $"IT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

    private static InstantTransferResponse MapToResponse(InstantTransfer t) => new(
        t.Id,
        t.Reference,
        new RecipientDto(
            t.RecipientMethod,
            t.RecipientIdentifier,
            t.RecipientResolvedName,
            t.RecipientBankName,
            t.RecipientNaitrustAccountNumber,
            t.RecipientNaitrustId,
            t.RecipientAccountType,
            t.RecipientIdentityVerified),
        t.AmountMinor,
        t.Currency,
        t.FeeMinor,
        t.Narration,
        t.Status,
        t.Provider,
        t.IsMock,
        t.CreatedAt,
        t.CompletedAt);
}
