namespace Naitrust.Domain.Models.Entities;

public class DealMessage : BaseEntity
{
    public Guid DealId { get; set; }
    public Guid SenderUserId { get; set; }
    public string SenderName { get; set; } = default!;
    public string Body { get; set; } = default!;
}
