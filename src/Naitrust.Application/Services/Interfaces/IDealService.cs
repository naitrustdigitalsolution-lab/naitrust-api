using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Security;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IDealService
{
    /// <summary>
    /// Creates a new deal in Draft status and assigns the creator as the initiating party.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> CreateDealAsync(Guid userId, CreateDealRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a deal by ID, including parties, agreement, milestones, and allowed actions.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> GetDealAsync(Guid dealId, CancellationToken ct = default);

    /// <summary>
    /// Lists all deals where the authenticated user is a party, with pagination support.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<DealResponse>>> ListDealsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Updates a deal's title, description, or amount (only allowed while in Draft status).
    /// </summary>
    Task<NaitrustResponse<DealResponse>> UpdateDealAsync(Guid dealId, UpdateDealRequest request, CancellationToken ct = default);

    /// <summary>
    /// Returns all available deal types (e.g., Goods, Services, Real Estate).
    /// </summary>
    Task<NaitrustResponse<List<DealTypeResponse>>> GetDealTypesAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns a single identity capture's photo URL for an authorized party to the deal,
    /// gated by the 90-day retention window (waived when under legal hold).
    /// </summary>
    Task<NaitrustResponse<DealIdentityCaptureResponse>> GetIdentityCaptureViewAsync(Guid dealId, Guid captureId, Guid callerUserId, CancellationToken ct = default);
}
