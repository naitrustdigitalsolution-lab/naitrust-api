using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class NaitrustRoleConfiguration : IEntityTypeConfiguration<NaitrustRole>
{
    public void Configure(EntityTypeBuilder<NaitrustRole> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasMany(r => r.RoleClaims).WithOne().HasForeignKey(rc => rc.RoleId);
    }
}
