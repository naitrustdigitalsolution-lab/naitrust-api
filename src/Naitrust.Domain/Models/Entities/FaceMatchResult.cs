using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class FaceMatchResult
{
    public Guid Id { get; set; }
    public Guid VerificationRequestId { get; set; }
    public string Provider { get; set; } = default!;
    public IdType IdType { get; set; }
    public string IdNumberHash { get; set; } = default!;
    public bool Match { get; set; }
    public decimal MatchScore { get; set; }
    public decimal Confidence { get; set; }
    public bool LivenessPassed { get; set; }
    public string? RawProviderResponse { get; set; }
    public DateTime CreatedAt { get; set; }
}
