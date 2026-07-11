namespace Naitrust.Domain.Models.Events;

public record EvidenceUploadedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    Guid EvidenceFileId,
    Guid UploadedByUserId) : IDomainEvent;
