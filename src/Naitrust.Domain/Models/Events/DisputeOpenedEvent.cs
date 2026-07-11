namespace Naitrust.Domain.Models.Events;

public record DisputeOpenedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    Guid DisputeId,
    Guid OpenedByUserId) : IDomainEvent;
