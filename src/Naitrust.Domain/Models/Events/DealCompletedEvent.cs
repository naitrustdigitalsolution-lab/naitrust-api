namespace Naitrust.Domain.Models.Events;

public record DealCompletedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid DealId) : IDomainEvent;
