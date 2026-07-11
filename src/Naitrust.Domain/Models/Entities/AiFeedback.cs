using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class AiFeedback
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public Guid UserId { get; set; }
    public AiFeedbackType FeedbackType { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
