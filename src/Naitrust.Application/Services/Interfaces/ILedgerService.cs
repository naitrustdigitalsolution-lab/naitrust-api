using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Interfaces;

public interface ILedgerService
{
    Task<NaitrustResponse<bool>> PostEntriesAsync(List<LedgerEntry> entries, CancellationToken ct = default);
    Task<NaitrustResponse<List<LedgerEntryResponse>>> GetEntriesByTransactionAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> ValidateBalanceAsync(Guid transactionId, CancellationToken ct = default);
}
