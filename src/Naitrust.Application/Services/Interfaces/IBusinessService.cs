using Naitrust.Domain.Models.Dtos.Requests.Businesses;
using Naitrust.Domain.Models.Dtos.Responses.Businesses;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IBusinessService
{
    /// <summary>
    /// Creates a new business entity and assigns the requesting user as the owner.
    /// </summary>
    Task<NaitrustResponse<BusinessResponse>> CreateBusinessAsync(Guid userId, CreateBusinessRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a business by its unique identifier.
    /// </summary>
    Task<NaitrustResponse<BusinessResponse>> GetBusinessAsync(Guid businessId, CancellationToken ct = default);

    /// <summary>
    /// Lists all businesses where the authenticated user is a member.
    /// </summary>
    Task<NaitrustResponse<List<BusinessResponse>>> GetMyBusinessesAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing business's details (name, description, etc.).
    /// </summary>
    Task<NaitrustResponse<BusinessResponse>> UpdateBusinessAsync(Guid businessId, UpdateBusinessRequest request, CancellationToken ct = default);

    /// <summary>
    /// Adds a new member to a business with the specified role.
    /// </summary>
    Task<NaitrustResponse<BusinessMemberResponse>> AddMemberAsync(Guid businessId, AddBusinessMemberRequest request, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing business member's role or status.
    /// </summary>
    Task<NaitrustResponse<BusinessMemberResponse>> UpdateMemberAsync(Guid businessId, Guid memberId, UpdateBusinessMemberRequest request, CancellationToken ct = default);
}
