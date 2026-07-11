namespace Naitrust.Domain.Models.Events;

public record VerificationCompletedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid VerificationRequestId,
    string SubjectType,
    Guid SubjectId,
    string Status) : IDomainEvent;
