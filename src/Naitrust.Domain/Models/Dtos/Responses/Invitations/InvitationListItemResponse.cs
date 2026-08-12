namespace Naitrust.Domain.Models.Dtos.Responses.Invitations;

public record InvitationListItemResponse(
    Guid Id,
    Guid TransactionId,
    string InviterName,
    string YourRole,
    string Title,
    long AmountMinor,
    string Currency,
    string Status,
    DateTime ExpiresAt,
    DateTime CreatedAt);
