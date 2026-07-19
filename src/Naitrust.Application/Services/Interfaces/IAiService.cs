using Naitrust.Domain.Models.Dtos.Requests.Ai;
using Naitrust.Domain.Models.Dtos.Responses.Ai;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAiService
{
    /// <summary>
    /// Generates an AI-powered risk assessment for a transaction based on its details and parties.
    /// </summary>
    Task<NaitrustResponse<AiAssessmentResponse>> GetRiskAssessmentAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Generates an AI-recommended evidence checklist for a transaction type and context.
    /// </summary>
    Task<NaitrustResponse<AiAssessmentResponse>> GetEvidenceChecklistAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Generates an AI summary of a dispute, including key facts and recommended resolution.
    /// </summary>
    Task<NaitrustResponse<AiAssessmentResponse>> GetDisputeSummaryAsync(Guid disputeId, CancellationToken ct = default);

    /// <summary>
    /// Generates an AI summary of a verification request's completeness and risk indicators.
    /// </summary>
    Task<NaitrustResponse<AiAssessmentResponse>> GetVerificationSummaryAsync(Guid verificationId, CancellationToken ct = default);

    /// <summary>
    /// Generates an AI summary of a user's reputation profile and transaction history patterns.
    /// </summary>
    Task<NaitrustResponse<AiAssessmentResponse>> GetReputationSummaryAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Processes a natural-language query from an admin and returns AI-generated insights.
    /// </summary>
    Task<NaitrustResponse<AiAssessmentResponse>> GetAdminCopilotAsync(string query, CancellationToken ct = default);

    /// <summary>
    /// Submits user feedback on an AI assessment to improve future results.
    /// </summary>
    Task<NaitrustResponse<bool>> SubmitFeedbackAsync(Guid userId, AiFeedbackRequest request, CancellationToken ct = default);
}
