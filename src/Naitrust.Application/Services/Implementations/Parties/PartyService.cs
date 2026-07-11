using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Application.Services.Implementations.Parties;

public class PartyService : IPartyService
{
    public Task<NaitrustResponse<TransactionPartyResponse>> CreatePartyAsync(Guid transactionId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionPartyResponse>> GetPartyAsync(Guid partyId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<List<TransactionPartyResponse>>> GetPartiesByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionPartyResponse>> ResolvePartyAsync(Guid partyId, Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
