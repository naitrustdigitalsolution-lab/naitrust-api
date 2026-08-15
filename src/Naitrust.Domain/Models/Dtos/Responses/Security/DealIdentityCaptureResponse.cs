namespace Naitrust.Domain.Models.Dtos.Responses.Security;

public record DealIdentityCaptureResponse(
    Guid CaptureId,
    Guid DealId,
    Guid SubjectUserId,
    string RepresentativeName,
    string? BusinessName,
    string Action,
    DateTime CapturedAt,
    string VerificationStatus,
    bool PhotoAvailable,
    DateTime? RetentionExpiresAt,
    bool LegalHold,
    /// <summary>Only populated by the view-by-id endpoint, never by list/summary responses.</summary>
    string? PhotoUrl = null);
