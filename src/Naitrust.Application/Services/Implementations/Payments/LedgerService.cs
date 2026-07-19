using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Payments;

public class LedgerService : ILedgerService
{
    private readonly IUnitOfWork _unitOfWork;

    public LedgerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<bool>> PostEntriesAsync(List<LedgerEntry> entries, CancellationToken ct = default)
    {
        if (entries is null || entries.Count == 0)
        {
            return NaitrustResponse<bool>.BadRequest("Ledger entries cannot be empty.");
        }

        var totalDebits = entries.Sum(e => e.DebitMinor);
        var totalCredits = entries.Sum(e => e.CreditMinor);

        if (totalDebits != totalCredits)
        {
            return NaitrustResponse<bool>.BadRequest($"Ledger entries are not balanced. Debits: {totalDebits}, Credits: {totalCredits}.");
        }

        var entryGroupId = Guid.NewGuid();
        var repo = _unitOfWork.GetRepository<LedgerEntry>();

        foreach (var entry in entries)
        {
            entry.Id = Guid.NewGuid();
            entry.EntryGroupId = entryGroupId;
            entry.CreatedAt = DateTime.UtcNow;
        }

        await repo.AddRangeAsync(entries);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Ledger entries posted successfully.", true);
    }

    public async Task<NaitrustResponse<List<LedgerEntryResponse>>> GetEntriesByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<LedgerEntry>();

        var entries = await repo.GetAllDataAsync(
            predicate: e => e.TransactionId == transactionId,
            orderBy: q => q.OrderBy(e => e.CreatedAt));

        var responses = entries.Select(e => new LedgerEntryResponse(
            e.Id,
            e.EntryGroupId,
            e.EventType.ToString(),
            e.Account,
            e.DebitMinor,
            e.CreditMinor,
            e.Currency,
            e.Memo,
            e.CreatedAt)).ToList();

        return NaitrustResponse<List<LedgerEntryResponse>>.Success("Ledger entries retrieved successfully.", responses);
    }

    public async Task<NaitrustResponse<bool>> ValidateBalanceAsync(Guid transactionId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<LedgerEntry>();

        var entries = await repo.GetAllDataAsync(e => e.TransactionId == transactionId);
        var entryList = entries.ToList();

        if (entryList.Count == 0)
        {
            return NaitrustResponse<bool>.Success("No ledger entries found for transaction.", true);
        }

        var totalDebits = entryList.Sum(e => e.DebitMinor);
        var totalCredits = entryList.Sum(e => e.CreditMinor);

        var isBalanced = totalDebits == totalCredits;

        return isBalanced
            ? NaitrustResponse<bool>.Success("Ledger is balanced.", true)
            : NaitrustResponse<bool>.BadRequest($"Ledger is not balanced. Debits: {totalDebits}, Credits: {totalCredits}.");
    }
}
