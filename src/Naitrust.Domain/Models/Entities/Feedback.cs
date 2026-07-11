namespace Naitrust.Domain.Models.Entities;

public class Feedback : BaseEntity
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int Rating { get; set; }
    public string Message { get; set; } = default!;
}
