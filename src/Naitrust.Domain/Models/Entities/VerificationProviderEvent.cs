namespace Naitrust.Domain.Models.Entities;

public class VerificationProviderEvent
{
    public Guid Id { get; set; }
    public Guid VerificationRequestId { get; set; }
    public string Provider { get; set; } = default!;
    public string ProviderReference { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
