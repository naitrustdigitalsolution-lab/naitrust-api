using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Invitations;
using Naitrust.Domain.Models.Dtos.Responses.Invitations;

namespace Naitrust.Application.Services.Interfaces;

public interface IInvitationService
{
    Task<NaitrustResponse<List<InvitationListItemResponse>>> ListAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<DealInvitationResponse>> GetAsync(Guid invitationId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<PublicInvitationPreviewResponse>> GetPublicPreviewAsync(string token, CancellationToken ct = default);
    Task<NaitrustResponse<DealInvitationResponse>> ClaimAsync(string token, ClaimInvitationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<DealInvitationResponse>> RespondAsync(Guid invitationId, Guid userId, string action, CancellationToken ct = default);
}
