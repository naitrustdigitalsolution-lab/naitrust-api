namespace Naitrust.Domain.Models.Events;

public record TransactionStatusChangedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    string OldStatus,
    string NewStatus,
    Guid ChangedByUserId) : IDomainEvent;
