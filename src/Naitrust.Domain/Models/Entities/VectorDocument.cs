namespace Naitrust.Domain.Models.Entities;

public class VectorDocument
{
    public Guid Id { get; set; }
    public string SourceType { get; set; } = default!;
    public Guid SourceId { get; set; }
    public string EmbeddingModel { get; set; } = default!;
    public string? Embedding { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
}
