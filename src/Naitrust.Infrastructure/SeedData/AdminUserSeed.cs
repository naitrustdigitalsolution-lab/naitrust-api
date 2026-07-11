using Microsoft.EntityFrameworkCore;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;

namespace Naitrust.Infrastructure.SeedData;

public static class AdminUserSeed
{
    public static void SeedAdminUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Email = "admin@naitrust.com",
                FirstName = "Naitrust",
                LastName = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? "ChangeMe!123"),
                Role = UserRole.SuperAdmin,
                Status = UserStatus.Active,
                EmailVerifiedAt = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
