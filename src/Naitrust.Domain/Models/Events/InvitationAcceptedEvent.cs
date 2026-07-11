namespace Naitrust.Domain.Models.Events;

public record InvitationAcceptedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    Guid UserId) : IDomainEvent;
