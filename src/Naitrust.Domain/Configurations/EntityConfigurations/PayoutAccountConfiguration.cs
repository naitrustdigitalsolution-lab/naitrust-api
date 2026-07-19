using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class PayoutAccountConfiguration : IEntityTypeConfiguration<PayoutAccount>
{
    public void Configure(EntityTypeBuilder<PayoutAccount> builder)
    {
        builder.ToTable("PayoutAccounts");

        builder.Property(x => x.BankCode).HasMaxLength(10).IsRequired();
        builder.Property(x => x.AccountNumberHash).HasMaxLength(512).IsRequired();
        builder.Property(x => x.BankName).HasMaxLength(100);
        builder.Property(x => x.AccountName).HasMaxLength(200);
        builder.Property(x => x.ProviderReference).HasMaxLength(200);

        builder.Property(x => x.NameMatchStatus).HasConversion<string>();

        builder.HasIndex(x => x.PartyId);

        builder.HasOne<Party>().WithMany().HasForeignKey(x => x.PartyId).OnDelete(DeleteBehavior.Cascade);
    }
}
