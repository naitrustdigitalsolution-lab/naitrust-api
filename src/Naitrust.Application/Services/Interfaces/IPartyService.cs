using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IPartyService
{
    Task<NaitrustResponse<TransactionPartyResponse>> CreatePartyAsync(Guid transactionId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionPartyResponse>> GetPartyAsync(Guid partyId, CancellationToken ct = default);
    Task<NaitrustResponse<List<TransactionPartyResponse>>> GetPartiesByTransactionAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionPartyResponse>> ResolvePartyAsync(Guid partyId, Guid userId, CancellationToken ct = default);
}
