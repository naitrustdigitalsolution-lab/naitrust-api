using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Wallet;
using Naitrust.Domain.Models.Dtos.Responses.Wallet;

namespace Naitrust.Application.Services.Interfaces;

public interface IWalletService
{
    /// <summary>Returns the user's wallet with live balance from Anchor.</summary>
    Task<NaitrustResponse<WalletAccountResponse>> GetWalletAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Lists all linked bank accounts for the user.</summary>
    Task<NaitrustResponse<List<LinkedBankAccountResponse>>> GetLinkedBankAccountsAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Validates and saves a new bank account for withdrawal. Runs Anchor name enquiry.</summary>
    Task<NaitrustResponse<LinkedBankAccountResponse>> AddLinkedBankAccountAsync(Guid userId, AddLinkedBankAccountRequest request, CancellationToken ct = default);

    /// <summary>Marks a linked bank account as the user's default for withdrawal.</summary>
    Task<NaitrustResponse<bool>> SetDefaultBankAccountAsync(Guid userId, Guid accountId, CancellationToken ct = default);

    /// <summary>Removes a linked bank account.</summary>
    Task<NaitrustResponse<bool>> RemoveLinkedBankAccountAsync(Guid userId, Guid accountId, CancellationToken ct = default);

    /// <summary>Withdraws funds from the user's wallet subledger to their linked bank account via NIP.</summary>
    Task<NaitrustResponse<WalletAccountResponse>> WithdrawAsync(Guid userId, WithdrawRequest request, CancellationToken ct = default);

    /// <summary>Returns the user's wallet activity feed (withdrawals, deposits, etc.).</summary>
    Task<NaitrustResponse<List<WalletActivityResponse>>> GetActivityAsync(Guid userId, CancellationToken ct = default);
}
