namespace Naitrust.Domain.Models.Entities;

public class OutboxMessage : BaseEntity
{
    public string EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime? ProcessedAt { get; set; }
}
