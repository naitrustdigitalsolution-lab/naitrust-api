using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IPartyService
{
    /// <summary>
    /// Creates a new deal party record linking a user to a deal.
    /// </summary>
    Task<NaitrustResponse<DealPartyResponse>> CreatePartyAsync(Guid dealId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a deal party by its unique identifier.
    /// </summary>
    Task<NaitrustResponse<DealPartyResponse>> GetPartyAsync(Guid partyId, CancellationToken ct = default);

    /// <summary>
    /// Lists all parties associated with a given deal.
    /// </summary>
    Task<NaitrustResponse<List<DealPartyResponse>>> GetPartiesByDealAsync(Guid dealId, CancellationToken ct = default);

    /// <summary>
    /// Resolves a party's identity by linking them to an existing user account.
    /// </summary>
    Task<NaitrustResponse<DealPartyResponse>> ResolvePartyAsync(Guid partyId, Guid userId, CancellationToken ct = default);
}
