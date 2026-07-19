using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;

namespace Naitrust.Infrastructure.SeedData;

public static class AdminUserSeed
{
    public static void SeedAdminUser(ModelBuilder modelBuilder)
    {
        var adminUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var superAdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000010");

        var hasher = new PasswordHasher<NaitrustUser>();

        var adminUser = new NaitrustUser
        {
            Id = adminUserId,
            UserName = "admin@naitrust.com",
            NormalizedUserName = "ADMIN@NAITRUST.COM",
            Email = "admin@naitrust.com",
            NormalizedEmail = "ADMIN@NAITRUST.COM",
            EmailConfirmed = true,
            FirstName = "Naitrust",
            LastName = "Admin",
            Status = UserStatus.Active,
            EmailVerifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            SecurityStamp = Guid.NewGuid().ToString()
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser,
            Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? "ChangeMe!123");

        modelBuilder.Entity<NaitrustUser>().HasData(adminUser);
    }
}
