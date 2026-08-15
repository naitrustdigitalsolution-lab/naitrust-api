using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

/// <summary>
/// Liveness photo evidence captured at a deal-scoped action (creation, acceptance).
/// RetentionExpiresAt is intentionally not stored — computed at response time from
/// the deal's terminal-status timestamp, same as the frontend reference implementation.
/// </summary>
public class DealIdentityCapture : BaseEntity
{
    public Guid DealId { get; set; }
    public Guid SubjectUserId { get; set; }
    /// <summary>The client-generated capture id the frontend sent (browser-session reference, not a server id). Kept for traceability.</summary>
    public string? ClientCaptureId { get; set; }
    public string RepresentativeName { get; set; } = default!;
    public string? BusinessName { get; set; }
    public DealIdentityCaptureAction Action { get; set; }
    public DateTime CapturedAt { get; set; }
    public LivenessCaptureStatus VerificationStatus { get; set; } = LivenessCaptureStatus.Passed;
    /// <summary>Storage URL for the captured photo once real capture is wired up. Null when only referenced by client-side capture id.</summary>
    public string? EncryptedEvidenceRef { get; set; }
    public bool PhotoAvailable { get; set; }
    public bool LegalHold { get; set; }
}
