namespace Naitrust.Domain.Models.Dtos.Responses.Invitations;

public record PublicInvitationPreviewResponse(
    string Token,
    Guid InvitationId,
    string? Reference,
    string InviterName,
    string FromRole,
    string YourRole,
    string PartyMode,
    string Title,
    long AmountMinor,
    string Currency,
    string? Message,
    bool InviterVerified,
    string? InviterAccountType,
    string? IntendedAccountType,
    string? MaskedContact,
    DateTime ExpiresAt,
    string Status);
