namespace Naitrust.Domain.Models.Entities;

public class NewsletterSubscriber : BaseEntity
{
    public string Email { get; set; } = default!;
}
