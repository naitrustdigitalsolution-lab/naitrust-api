namespace Naitrust.Domain.Models.Entities;

public class DisputeMessage
{
    public Guid Id { get; set; }
    public Guid DisputeId { get; set; }
    public Guid SenderUserId { get; set; }
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
