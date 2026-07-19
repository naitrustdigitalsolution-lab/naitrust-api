using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Ai;
using Naitrust.Domain.Models.Dtos.Responses.Ai;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Ai;

public class AiIntelligenceService : IAiService
{
    private readonly IUnitOfWork _unitOfWork;

    public AiIntelligenceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetRiskAssessmentAsync(Guid transactionId, CancellationToken ct = default)
    {
        // TODO: Integrate with OpenAI provider when configured
        var response = new AiAssessmentResponse(
            Guid.NewGuid(),
            AssessmentType.RiskScore.ToString(),
            "AI risk assessment is not yet configured.",
            null, null, null, null, null,
            DateTime.UtcNow);
        return Task.FromResult(NaitrustResponse<AiAssessmentResponse>.Success("AI service not yet configured.", response));
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetEvidenceChecklistAsync(Guid transactionId, CancellationToken ct = default)
    {
        var response = new AiAssessmentResponse(
            Guid.NewGuid(),
            AssessmentType.EvidenceChecklist.ToString(),
            "AI evidence checklist is not yet configured.",
            null, null, null, null, null,
            DateTime.UtcNow);
        return Task.FromResult(NaitrustResponse<AiAssessmentResponse>.Success("AI service not yet configured.", response));
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetDisputeSummaryAsync(Guid disputeId, CancellationToken ct = default)
    {
        var response = new AiAssessmentResponse(
            Guid.NewGuid(),
            AssessmentType.DisputeSummary.ToString(),
            "AI dispute summary is not yet configured.",
            null, null, null, null, null,
            DateTime.UtcNow);
        return Task.FromResult(NaitrustResponse<AiAssessmentResponse>.Success("AI service not yet configured.", response));
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetVerificationSummaryAsync(Guid verificationId, CancellationToken ct = default)
    {
        var response = new AiAssessmentResponse(
            Guid.NewGuid(),
            AssessmentType.VerificationSummary.ToString(),
            "AI verification summary is not yet configured.",
            null, null, null, null, null,
            DateTime.UtcNow);
        return Task.FromResult(NaitrustResponse<AiAssessmentResponse>.Success("AI service not yet configured.", response));
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetReputationSummaryAsync(Guid userId, CancellationToken ct = default)
    {
        var response = new AiAssessmentResponse(
            Guid.NewGuid(),
            AssessmentType.ReputationSummary.ToString(),
            "AI reputation summary is not yet configured.",
            null, null, null, null, null,
            DateTime.UtcNow);
        return Task.FromResult(NaitrustResponse<AiAssessmentResponse>.Success("AI service not yet configured.", response));
    }

    public Task<NaitrustResponse<AiAssessmentResponse>> GetAdminCopilotAsync(string query, CancellationToken ct = default)
    {
        var response = new AiAssessmentResponse(
            Guid.NewGuid(),
            "AdminCopilot",
            "AI admin copilot is not yet configured.",
            null, null, null, null, null,
            DateTime.UtcNow);
        return Task.FromResult(NaitrustResponse<AiAssessmentResponse>.Success("AI service not yet configured.", response));
    }

    public async Task<NaitrustResponse<bool>> SubmitFeedbackAsync(Guid userId, AiFeedbackRequest request, CancellationToken ct = default)
    {
        if (!Enum.TryParse<AiFeedbackType>(request.FeedbackType, true, out var feedbackType))
        {
            return NaitrustResponse<bool>.BadRequest("Invalid feedback type.");
        }

        var repo = _unitOfWork.GetRepository<AiFeedback>();
        var feedback = new AiFeedback
        {
            Id = Guid.NewGuid(),
            AssessmentId = request.AssessmentId,
            UserId = userId,
            FeedbackType = feedbackType,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(feedback);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Feedback submitted.", true);
    }
}
