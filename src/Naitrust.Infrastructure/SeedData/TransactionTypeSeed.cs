using Microsoft.EntityFrameworkCore;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Infrastructure.SeedData;

public static class TransactionTypeSeed
{
    public static void SeedTransactionTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionType>().HasData(
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567801"),
                Key = "import",
                Name = "Import Transaction",
                RequiredVerificationLevel = VerificationLevel.Enhanced,
                ReleaseMode = ReleaseMode.Single,
                AutoConfirmWindowHours = 72,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567802"),
                Key = "export",
                Name = "Export Transaction",
                RequiredVerificationLevel = VerificationLevel.Enhanced,
                ReleaseMode = ReleaseMode.Single,
                AutoConfirmWindowHours = 72,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567803"),
                Key = "freelance",
                Name = "Freelance Project",
                RequiredVerificationLevel = VerificationLevel.Standard,
                ReleaseMode = ReleaseMode.Single,
                AutoConfirmWindowHours = 48,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567804"),
                Key = "real_estate",
                Name = "Real Estate Transaction",
                RequiredVerificationLevel = VerificationLevel.Enhanced,
                ReleaseMode = ReleaseMode.Milestone,
                AutoConfirmWindowHours = 168,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567805"),
                Key = "general_goods",
                Name = "General Goods Purchase",
                RequiredVerificationLevel = VerificationLevel.Basic,
                ReleaseMode = ReleaseMode.Single,
                AutoConfirmWindowHours = 48,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567806"),
                Key = "service",
                Name = "Service Agreement",
                RequiredVerificationLevel = VerificationLevel.Standard,
                ReleaseMode = ReleaseMode.Single,
                AutoConfirmWindowHours = 48,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new TransactionType
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567807"),
                Key = "vehicle",
                Name = "Vehicle Purchase",
                RequiredVerificationLevel = VerificationLevel.Enhanced,
                ReleaseMode = ReleaseMode.Single,
                AutoConfirmWindowHours = 72,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
