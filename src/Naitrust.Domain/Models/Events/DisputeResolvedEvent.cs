namespace Naitrust.Domain.Models.Events;

public record DisputeResolvedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid DisputeId,
    string Resolution) : IDomainEvent;
