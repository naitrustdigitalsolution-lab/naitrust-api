using Naitrust.Domain.Models.Enums;

namespace Naitrust.Domain.Models.Entities;

public class AiPromptVersion : BaseEntity
{
    public string Name { get; set; } = default!;
    public int Version { get; set; }
    public string? Purpose { get; set; }
    public string? Schema { get; set; }
    public PromptVersionStatus Status { get; set; }
}
