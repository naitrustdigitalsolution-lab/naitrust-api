namespace Naitrust.Domain.Models.Events;

public record VerificationStepUpdatedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid VerificationRequestId,
    string Step,
    string Status) : IDomainEvent;
