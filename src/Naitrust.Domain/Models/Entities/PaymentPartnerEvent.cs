using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Domain.Models.Entities;

public class PaymentPartnerEvent
{
    public Guid Id { get; set; }
    public Guid VirtualAccountId { get; set; }
    public PaymentPartnerId Partner { get; set; }
    public string ProviderEventId { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime? ProcessedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
