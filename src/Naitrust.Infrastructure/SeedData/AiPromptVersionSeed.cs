using Microsoft.EntityFrameworkCore;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;

namespace Naitrust.Infrastructure.SeedData;

public static class AiPromptVersionSeed
{
    public static void SeedPromptVersions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiPromptVersion>().HasData(
            new AiPromptVersion
            {
                Id = Guid.Parse("b1b2c3d4-e5f6-7890-abcd-ef1234567801"),
                Name = "transaction_risk_assessment",
                Version = 1,
                Purpose = "Assess risk level for a transaction based on parties, amount, category, and verification status",
                Status = PromptVersionStatus.Draft,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AiPromptVersion
            {
                Id = Guid.Parse("b1b2c3d4-e5f6-7890-abcd-ef1234567802"),
                Name = "dispute_summary",
                Version = 1,
                Purpose = "Generate a neutral dispute summary for admin review",
                Status = PromptVersionStatus.Draft,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AiPromptVersion
            {
                Id = Guid.Parse("b1b2c3d4-e5f6-7890-abcd-ef1234567803"),
                Name = "evidence_checklist",
                Version = 1,
                Purpose = "Generate evidence completeness checklist for a transaction category",
                Status = PromptVersionStatus.Draft,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AiPromptVersion
            {
                Id = Guid.Parse("b1b2c3d4-e5f6-7890-abcd-ef1234567804"),
                Name = "verification_mismatch_summary",
                Version = 1,
                Purpose = "Summarize verification mismatches for admin review",
                Status = PromptVersionStatus.Draft,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
