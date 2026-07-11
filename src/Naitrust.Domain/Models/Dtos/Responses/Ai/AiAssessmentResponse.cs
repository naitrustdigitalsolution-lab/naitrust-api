namespace Naitrust.Domain.Models.Dtos.Responses.Ai;

public record AiAssessmentResponse(
    Guid Id,
    string AssessmentType,
    string? Summary,
    string? RiskLevel,
    decimal? Confidence,
    List<string>? Reasons,
    List<string>? Recommendations,
    List<string>? EvidenceRefs,
    DateTime CreatedAt);
