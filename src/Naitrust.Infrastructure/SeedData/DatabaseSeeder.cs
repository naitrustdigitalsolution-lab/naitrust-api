using Microsoft.EntityFrameworkCore;

namespace Naitrust.Infrastructure.SeedData;

public class DatabaseSeeder
{
    public void SeedAll(ModelBuilder modelBuilder)
    {
        TransactionTypeSeed.SeedTransactionTypes(modelBuilder);
        AdminUserSeed.SeedAdminUser(modelBuilder);
        AiPromptVersionSeed.SeedPromptVersions(modelBuilder);
    }
}
