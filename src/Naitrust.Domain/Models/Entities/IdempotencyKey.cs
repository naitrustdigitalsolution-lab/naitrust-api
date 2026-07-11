namespace Naitrust.Domain.Models.Entities;

public class IdempotencyKey
{
    public Guid Id { get; set; }
    public string Key { get; set; } = default!;
    public string? Scope { get; set; }
    public string? RequestHash { get; set; }
    public string? ResponseBody { get; set; }
    public int? StatusCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
