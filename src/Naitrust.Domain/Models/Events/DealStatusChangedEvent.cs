namespace Naitrust.Domain.Models.Events;

public record DealStatusChangedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid DealId,
    string OldStatus,
    string NewStatus,
    Guid ChangedByUserId) : IDomainEvent;
