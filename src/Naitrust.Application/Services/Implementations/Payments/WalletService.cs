using Naitrust.Application.ExternalServices;
using Naitrust.Application.ExternalServices.Anchor;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Wallet;
using Naitrust.Domain.Models.Dtos.Responses.Wallet;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Payments;

public class WalletService : IWalletService
{
    private const long AccountLimitMinor = 500_000_000L; // ₦5,000,000

    private readonly IUnitOfWork _unitOfWork;
    private readonly AnchorPaymentPartner _anchor;

    public WalletService(IUnitOfWork unitOfWork, AnchorPaymentPartner anchor)
    {
        _unitOfWork = unitOfWork;
        _anchor = anchor;
    }

    // ── Get wallet ────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<WalletAccountResponse>> GetWalletAsync(
        Guid userId, CancellationToken ct = default)
    {
        var vaRepo = _unitOfWork.GetRepository<VirtualAccount>();
        var wallet = await vaRepo.GetSingleByAsync(
            va => va.UserId == userId && va.Type == VirtualAccountType.Settlement && !va.IsDeleted);

        if (wallet is null)
            return NaitrustResponse<WalletAccountResponse>.NotFound(
                "Wallet not found. Please complete KYC to activate your wallet.");

        var response = await BuildWalletResponseAsync(wallet, userId, ct);
        return NaitrustResponse<WalletAccountResponse>.Success("Wallet retrieved.", response);
    }

    // ── Linked bank accounts ──────────────────────────────────────────────────

    public async Task<NaitrustResponse<List<LinkedBankAccountResponse>>> GetLinkedBankAccountsAsync(
        Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<LinkedBankAccount>();
        var accounts = await repo.GetAllDataAsync(
            a => a.UserId == userId && !a.IsDeleted,
            orderBy: q => q.OrderByDescending(a => a.IsDefault).ThenBy(a => a.CreatedAt));

        return NaitrustResponse<List<LinkedBankAccountResponse>>.Success(
            "Bank accounts retrieved.",
            accounts.Select(MapToLinkedBankAccountResponse).ToList());
    }

    public async Task<NaitrustResponse<LinkedBankAccountResponse>> AddLinkedBankAccountAsync(
        Guid userId, AddLinkedBankAccountRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.BankCode) || string.IsNullOrWhiteSpace(request.AccountNumber))
            return NaitrustResponse<LinkedBankAccountResponse>.BadRequest(
                "BankCode and AccountNumber are required.");

        // Validate via Anchor name enquiry
        PayoutAccountValidationResult validation;
        try
        {
            validation = await _anchor.ValidatePayoutAccountAsync(
                new ValidatePayoutAccountPartnerRequest(request.BankCode, request.AccountNumber), ct);
        }
        catch (Exception ex)
        {
            return NaitrustResponse<LinkedBankAccountResponse>.BadRequest(
                $"Bank account validation failed: {ex.Message}");
        }

        var repo = _unitOfWork.GetRepository<LinkedBankAccount>();

        // Check for duplicate
        var existing = await repo.GetSingleByAsync(
            a => a.UserId == userId && a.AccountNumber == request.AccountNumber
                 && a.BankCode == request.BankCode && !a.IsDeleted);

        if (existing is not null)
            return NaitrustResponse<LinkedBankAccountResponse>.Conflict(
                "This bank account is already linked.");

        // If setting as default, clear others
        if (request.SetAsDefault)
        {
            var others = await repo.GetAllDataAsync(a => a.UserId == userId && a.IsDefault && !a.IsDeleted);
            foreach (var other in others)
            {
                other.IsDefault = false;
                await repo.UpdateAsync(other);
            }
        }

        var account = new LinkedBankAccount
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BankCode = request.BankCode,
            BankName = validation.BankName,
            AccountNumber = request.AccountNumber,
            AccountName = validation.AccountName,
            IsDefault = request.SetAsDefault,
            VerifiedAt = DateTime.UtcNow,
            IsActive = true
        };

        await repo.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<LinkedBankAccountResponse>.Created(
            "Bank account added successfully.",
            MapToLinkedBankAccountResponse(account));
    }

    public async Task<NaitrustResponse<bool>> SetDefaultBankAccountAsync(
        Guid userId, Guid accountId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<LinkedBankAccount>();
        var target = await repo.GetByIdAsync(accountId);

        if (target is null || target.IsDeleted || target.UserId != userId)
            return NaitrustResponse<bool>.NotFound("Bank account not found.");

        var others = await repo.GetAllDataAsync(a => a.UserId == userId && a.IsDefault && !a.IsDeleted);
        foreach (var other in others)
        {
            other.IsDefault = false;
            await repo.UpdateAsync(other);
        }

        target.IsDefault = true;
        await repo.UpdateAsync(target);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Default bank account updated.", true);
    }

    public async Task<NaitrustResponse<bool>> RemoveLinkedBankAccountAsync(
        Guid userId, Guid accountId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<LinkedBankAccount>();
        var account = await repo.GetByIdAsync(accountId);

        if (account is null || account.IsDeleted || account.UserId != userId)
            return NaitrustResponse<bool>.NotFound("Bank account not found.");

        account.IsDeleted = true;
        account.IsActive = false;
        await repo.UpdateAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Bank account removed.", true);
    }

    // ── Withdraw ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<WalletAccountResponse>> WithdrawAsync(
        Guid userId, WithdrawRequest request, CancellationToken ct = default)
    {
        if (request.AmountMinor <= 0)
            return NaitrustResponse<WalletAccountResponse>.BadRequest("Withdrawal amount must be greater than zero.");

        if (request.AmountMinor > AccountLimitMinor)
            return NaitrustResponse<WalletAccountResponse>.BadRequest(
                $"Amount exceeds the single-transaction limit of ₦{AccountLimitMinor / 100m:N2}.");

        // Verify bank account belongs to user
        var bankRepo = _unitOfWork.GetRepository<LinkedBankAccount>();
        var bankAccount = await bankRepo.GetByIdAsync(request.LinkedBankAccountId);

        if (bankAccount is null || bankAccount.IsDeleted || bankAccount.UserId != userId)
            return NaitrustResponse<WalletAccountResponse>.NotFound("Linked bank account not found.");

        // Fetch user's wallet subledger
        var vaRepo = _unitOfWork.GetRepository<VirtualAccount>();
        var wallet = await vaRepo.GetSingleByAsync(
            va => va.UserId == userId && va.Type == VirtualAccountType.Settlement && !va.IsDeleted);

        if (wallet is null || string.IsNullOrEmpty(wallet.ProviderReference))
            return NaitrustResponse<WalletAccountResponse>.BadRequest(
                "Wallet not set up. Please complete KYC first.");

        // Check live balance from Anchor
        var balance = await _anchor.GetFundingStatusAsync(
            new FundingStatusRequest(wallet.ProviderReference), ct);

        if (balance.AmountReceivedMinor < request.AmountMinor)
            return NaitrustResponse<WalletAccountResponse>.BadRequest(
                $"Insufficient balance. Available: ₦{balance.AmountReceivedMinor / 100m:N2}, " +
                $"Requested: ₦{request.AmountMinor / 100m:N2}.");

        // NIP transfer: user subledger → external bank
        var idempotencyKey = $"withdrawal-{userId}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        var transfer = await _anchor.WithdrawToExternalBankAsync(
            sourceSubAccountId: wallet.ProviderReference,
            amountMinor: request.AmountMinor,
            currency: wallet.Currency,
            destinationAccountNumber: bankAccount.AccountNumber,
            destinationBankCode: bankAccount.BankCode,
            narration: $"Naitrust withdrawal to {bankAccount.BankName}",
            idempotencyKey: idempotencyKey,
            ct: ct);

        // Record payment instruction
        var instrRepo = _unitOfWork.GetRepository<PaymentInstruction>();
        await instrRepo.AddAsync(new PaymentInstruction
        {
            Id = Guid.NewGuid(),
            TransactionId = Guid.Empty, // not deal-specific
            VirtualAccountId = wallet.Id,
            InstructionType = PaymentInstructionType.Withdrawal,
            Partner = PaymentPartnerId.Anchor,
            IdempotencyKey = idempotencyKey,
            Status = PaymentInstructionStatus.Sent,
            PartnerReference = transfer.PartnerReference,
            IsActive = true
        });

        // Log wallet activity
        var activityRepo = _unitOfWork.GetRepository<WalletActivity>();
        await activityRepo.AddAsync(new WalletActivity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Kind = "withdrawal",
            AmountMinor = request.AmountMinor,
            Currency = wallet.Currency,
            Description = $"Withdrawn to {bankAccount.BankName} ···· {bankAccount.AccountNumber[^4..]}",
            IsActive = true
        });

        await _unitOfWork.SaveChangesAsync();

        // Return fresh wallet state (balance will be reduced on Anchor's side)
        var response = await BuildWalletResponseAsync(wallet, userId, ct);
        return NaitrustResponse<WalletAccountResponse>.Success(
            "Withdrawal initiated successfully.", response);
    }

    // ── Activity ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<List<WalletActivityResponse>>> GetActivityAsync(
        Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<WalletActivity>();
        var activities = await repo.GetAllDataAsync(
            a => a.UserId == userId && !a.IsDeleted,
            orderBy: q => q.OrderByDescending(a => a.CreatedAt));

        return NaitrustResponse<List<WalletActivityResponse>>.Success(
            "Activity retrieved.",
            activities.Select(a => new WalletActivityResponse(
                a.Id, a.Kind, a.AmountMinor, a.Currency, a.Description, a.CreatedAt)).ToList());
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<WalletAccountResponse> BuildWalletResponseAsync(
        VirtualAccount wallet, Guid userId, CancellationToken ct)
    {
        // Live balance from Anchor
        long availableMinor = 0;
        if (!string.IsNullOrEmpty(wallet.ProviderReference))
        {
            try
            {
                var balanceResult = await _anchor.GetFundingStatusAsync(
                    new FundingStatusRequest(wallet.ProviderReference), ct);
                availableMinor = balanceResult.AmountReceivedMinor;
            }
            catch
            {
                availableMinor = 0; // degrade gracefully if Anchor is unavailable
            }
        }

        // Protected = sum of deal amounts currently held in platform escrow for this buyer
        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var fundedDeals = await dealRepo.GetAllDataAsync(
            d => d.CreatedByUserId == userId
                 && d.PaymentStatus == PaymentStatus.PaymentConfirmedByPartner
                 && !d.IsDeleted);

        // Check buyer party more accurately — user might be the creator but as seller
        var partyRepo = _unitOfWork.GetRepository<DealParty>();
        long protectedMinor = 0;
        foreach (var deal in fundedDeals)
        {
            var party = await partyRepo.GetSingleByAsync(
                p => p.DealId == deal.Id && p.UserId == userId
                     && p.PartyType == PartyType.Buyer && !p.IsDeleted);
            if (party is not null)
                protectedMinor += deal.AmountMinor;
        }

        // Totals from activity log
        var activityRepo = _unitOfWork.GetRepository<WalletActivity>();
        var allActivity = await activityRepo.GetAllDataAsync(a => a.UserId == userId && !a.IsDeleted);
        var actList = allActivity.ToList();

        var totalInflowMinor = actList
            .Where(a => a.Kind is "funding" or "instant_transfer_in" or "protected_release")
            .Sum(a => a.AmountMinor);

        var totalOutflowMinor = actList
            .Where(a => a.Kind is "withdrawal" or "protected_allocation")
            .Sum(a => a.AmountMinor);

        VirtualAccountInfoDto? vaInfo = null;
        if (!string.IsNullOrEmpty(wallet.AccountNumber))
        {
            vaInfo = new VirtualAccountInfoDto(
                wallet.BankName ?? "",
                wallet.AccountNumber,
                wallet.AccountName ?? "");
        }

        return new WalletAccountResponse(
            Id: wallet.Id,
            OwnerUserId: userId,
            BusinessId: wallet.BusinessId,
            Balance: new WalletBalanceDto(availableMinor, PendingMinor: 0, protectedMinor, wallet.Currency),
            TotalInflowMinor: totalInflowMinor,
            TotalOutflowMinor: totalOutflowMinor,
            AccountLimitMinor: AccountLimitMinor,
            VirtualAccount: vaInfo,
            CreatedAt: wallet.CreatedAt);
    }

    private static LinkedBankAccountResponse MapToLinkedBankAccountResponse(LinkedBankAccount a) =>
        new(a.Id, a.BankName, a.AccountNumber, a.AccountName, a.IsDefault);
}
