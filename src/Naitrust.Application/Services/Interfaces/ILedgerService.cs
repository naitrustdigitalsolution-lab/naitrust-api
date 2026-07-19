using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Interfaces;

public interface ILedgerService
{
    /// <summary>
    /// Posts a balanced set of double-entry ledger entries (sum of debits must equal sum of credits).
    /// </summary>
    Task<NaitrustResponse<bool>> PostEntriesAsync(List<LedgerEntry> entries, CancellationToken ct = default);

    /// <summary>
    /// Retrieves all ledger entries for a transaction, ordered by creation date.
    /// </summary>
    Task<NaitrustResponse<List<LedgerEntryResponse>>> GetEntriesByTransactionAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Validates that all ledger entries for a transaction are balanced (net sum equals zero).
    /// </summary>
    Task<NaitrustResponse<bool>> ValidateBalanceAsync(Guid transactionId, CancellationToken ct = default);
}
