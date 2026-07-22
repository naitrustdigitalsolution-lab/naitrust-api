namespace Naitrust.Domain.Models.Entities;

public class ReportedConcern : BaseEntity
{
    public string? Name { get; set; }
    public string Email { get; set; } = default!;
    public string? Category { get; set; }
    public string? Description { get; set; }
}
