using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VirtualAccountConfiguration : IEntityTypeConfiguration<VirtualAccount>
{
    public void Configure(EntityTypeBuilder<VirtualAccount> builder)
    {
        builder.ToTable("VirtualAccounts");

        builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        builder.Property(x => x.ProviderReference).HasMaxLength(200);
        builder.Property(x => x.AccountNumber).HasMaxLength(20);
        builder.Property(x => x.AccountName).HasMaxLength(200);
        builder.Property(x => x.BankName).HasMaxLength(100);

        builder.Property(x => x.Partner).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Type).HasConversion<string>();

        builder.HasIndex(x => x.AccountNumber).IsUnique();
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.BusinessId);
        builder.HasIndex(x => x.Status);

        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Restrict);
    }
}
