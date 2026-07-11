using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Ai;
using Naitrust.Domain.Models.Dtos.Responses.Ai;

namespace Naitrust.Application.Services.Implementations.Ai;

public class AiIntelligenceService : IAiService
{
    public Task<NaitrustResponse<AiAssessmentResponse>> GetRiskAssessmentAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetEvidenceChecklistAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetDisputeSummaryAsync(Guid disputeId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetVerificationSummaryAsync(Guid verificationId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetReputationSummaryAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetAdminCopilotAsync(string query, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> SubmitFeedbackAsync(Guid userId, AiFeedbackRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
