using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Infrastructure.SeedData;

public static class RoleSeed
{
    public static void SeedRoles(ModelBuilder modelBuilder)
    {
        var superAdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000010");
        var adminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000011");
        var userRoleId = Guid.Parse("00000000-0000-0000-0000-000000000012");

        modelBuilder.Entity<NaitrustRole>().HasData(
            new NaitrustRole
            {
                Id = superAdminRoleId,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Description = "Full system access with all administrative privileges",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new NaitrustRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrative access for managing users and transactions",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new NaitrustRole
            {
                Id = userRoleId,
                Name = "User",
                NormalizedName = "USER",
                Description = "Standard user with access to create and participate in transactions",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Assign SuperAdmin role to the admin user
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                RoleId = superAdminRoleId
            }
        );

        // Seed role claims for SuperAdmin
        modelBuilder.Entity<NaitrustRoleClaim>().HasData(
            new NaitrustRoleClaim { Id = 1, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Users.Manage" },
            new NaitrustRoleClaim { Id = 2, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Transactions.Manage" },
            new NaitrustRoleClaim { Id = 3, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Disputes.Manage" },
            new NaitrustRoleClaim { Id = 4, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Verifications.Manage" },
            new NaitrustRoleClaim { Id = 5, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Payments.Manage" },
            new NaitrustRoleClaim { Id = 6, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Reports.View" },
            new NaitrustRoleClaim { Id = 7, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "Settings.Manage" },
            new NaitrustRoleClaim { Id = 8, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "AI.Manage" },

            // Admin role claims
            new NaitrustRoleClaim { Id = 9, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Manage" },
            new NaitrustRoleClaim { Id = 10, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Transactions.Manage" },
            new NaitrustRoleClaim { Id = 11, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Disputes.Manage" },
            new NaitrustRoleClaim { Id = 12, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Verifications.Manage" },
            new NaitrustRoleClaim { Id = 13, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Reports.View" },

            // User role claims
            new NaitrustRoleClaim { Id = 14, RoleId = userRoleId, ClaimType = "Permission", ClaimValue = "Transactions.Own" },
            new NaitrustRoleClaim { Id = 15, RoleId = userRoleId, ClaimType = "Permission", ClaimValue = "Disputes.Own" },
            new NaitrustRoleClaim { Id = 16, RoleId = userRoleId, ClaimType = "Permission", ClaimValue = "Verifications.Own" }
        );
    }
}
