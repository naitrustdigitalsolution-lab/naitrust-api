using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Domain.Models.Dtos.Responses.Invitations;

public record DealInvitationResponse(
    Guid Id,
    Guid TransactionId,
    string PublicToken,
    Guid? RecipientUserId,
    string? IntendedContact,
    string? IntendedAccountType,
    Guid? InviterProfileId,
    Guid? InviteeProfileId,
    string? PostAuthDestination,
    string FromName,
    string FromRole,
    string YourRole,
    string PartyMode,
    string Title,
    long AmountMinor,
    string Currency,
    string? Message,
    string? Reference,
    AgreementSnapshotDto? Agreement,
    string Status,
    DateTime ExpiresAt,
    DateTime CreatedAt);

public record AgreementSnapshotDto(List<AgreementSectionResponse> Sections);
