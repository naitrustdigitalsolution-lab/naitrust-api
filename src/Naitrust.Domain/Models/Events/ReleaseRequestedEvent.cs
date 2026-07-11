namespace Naitrust.Domain.Models.Events;

public record ReleaseRequestedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    Guid ReleaseRequestId) : IDomainEvent;
