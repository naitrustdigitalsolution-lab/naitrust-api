namespace Naitrust.Domain.Models.Events;

public record TransactionCompletedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId) : IDomainEvent;
