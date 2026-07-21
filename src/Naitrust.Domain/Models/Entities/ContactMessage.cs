namespace Naitrust.Domain.Models.Entities;

public class ContactMessage : BaseEntity
{
    public string? Name { get; set; }
    public string Email { get; set; } = default!;
    public string? Subject { get; set; }
    public string? Message { get; set; }
}
