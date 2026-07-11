namespace Naitrust.Domain.Models.Events;

public record TermsApprovedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    Guid AgreementId) : IDomainEvent;
