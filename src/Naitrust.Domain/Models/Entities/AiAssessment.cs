using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class AiAssessment
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = default!;
    public Guid EntityId { get; set; }
    public AssessmentType AssessmentType { get; set; }
    public string Model { get; set; } = default!;
    public string? PromptVersion { get; set; }
    public string? InputRefs { get; set; }
    public string Output { get; set; } = default!;
    public RiskLevel? RiskLevel { get; set; }
    public decimal? Confidence { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
