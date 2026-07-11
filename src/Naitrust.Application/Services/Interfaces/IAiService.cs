using Naitrust.Domain.Models.Dtos.Requests.Ai;
using Naitrust.Domain.Models.Dtos.Responses.Ai;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAiService
{
    Task<NaitrustResponse<AiAssessmentResponse>> GetRiskAssessmentAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<AiAssessmentResponse>> GetEvidenceChecklistAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<AiAssessmentResponse>> GetDisputeSummaryAsync(Guid disputeId, CancellationToken ct = default);
    Task<NaitrustResponse<AiAssessmentResponse>> GetVerificationSummaryAsync(Guid verificationId, CancellationToken ct = default);
    Task<NaitrustResponse<AiAssessmentResponse>> GetReputationSummaryAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<AiAssessmentResponse>> GetAdminCopilotAsync(string query, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> SubmitFeedbackAsync(Guid userId, AiFeedbackRequest request, CancellationToken ct = default);
}
