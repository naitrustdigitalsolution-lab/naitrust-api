using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Application.Services.Interfaces;

public interface IDealSubResourceService
{
    // Messages
    Task<NaitrustResponse<List<DealMessageResponse>>> GetMessagesAsync(Guid dealId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<DealMessageResponse>> SendMessageAsync(Guid dealId, Guid userId, SendDealMessageRequest request, CancellationToken ct = default);

    // Tracking
    Task<NaitrustResponse<List<TrackingMilestoneResponse>>> GetTrackingAsync(Guid dealId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<List<TrackingMilestoneResponse>>> AddTrackingStepAsync(Guid dealId, Guid userId, AddTrackingStepRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<List<TrackingMilestoneResponse>>> AdvanceTrackingAsync(Guid dealId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<List<TrackingMilestoneResponse>>> RevertTrackingAsync(Guid dealId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<List<TrackingMilestoneResponse>>> EditTrackingStepAsync(Guid dealId, Guid stepId, Guid userId, EditTrackingStepRequest request, CancellationToken ct = default);

    // Termination
    Task<NaitrustResponse<DealTerminationResponse?>> GetTerminationAsync(Guid dealId, Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<DealTerminationResponse>> RequestTerminationAsync(Guid dealId, Guid userId, RequestTerminationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<DealTerminationResponse>> RespondToTerminationAsync(Guid dealId, Guid userId, RespondTerminationRequest request, CancellationToken ct = default);
}
