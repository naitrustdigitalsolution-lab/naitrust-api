using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class NaitrustUserConfiguration : IEntityTypeConfiguration<NaitrustUser>
{
    public void Configure(EntityTypeBuilder<NaitrustUser> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);

        builder.Property(x => x.Bio).HasMaxLength(500);
        builder.Property(x => x.Address).HasMaxLength(250);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.Avatar).HasMaxLength(500);

        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => x.Status);
    }
}
