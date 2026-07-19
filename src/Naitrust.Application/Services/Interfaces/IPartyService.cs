using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IPartyService
{
    /// <summary>
    /// Creates a new transaction party record linking a user to a transaction.
    /// </summary>
    Task<NaitrustResponse<TransactionPartyResponse>> CreatePartyAsync(Guid transactionId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a transaction party by its unique identifier.
    /// </summary>
    Task<NaitrustResponse<TransactionPartyResponse>> GetPartyAsync(Guid partyId, CancellationToken ct = default);

    /// <summary>
    /// Lists all parties associated with a given transaction.
    /// </summary>
    Task<NaitrustResponse<List<TransactionPartyResponse>>> GetPartiesByTransactionAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Resolves a party's identity by linking them to an existing user account.
    /// </summary>
    Task<NaitrustResponse<TransactionPartyResponse>> ResolvePartyAsync(Guid partyId, Guid userId, CancellationToken ct = default);
}
