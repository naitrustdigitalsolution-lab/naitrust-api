namespace Naitrust.Domain.Models.Events;

public record PaymentReceivedEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid TransactionId,
    long AmountMinor,
    string PartnerId) : IDomainEvent;
