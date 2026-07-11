using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Guid ReviewerUserId { get; set; }
    public SubjectType RevieweeSubjectType { get; set; }
    public Guid RevieweeSubjectId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
