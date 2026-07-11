using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Implementations.Payments;

// Double-entry posting logic
public class LedgerService : ILedgerService
{
    public Task<NaitrustResponse<bool>> PostEntriesAsync(List<LedgerEntry> entries, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<List<LedgerEntryResponse>>> GetEntriesByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> ValidateBalanceAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
