using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class TransactionPartyConfiguration : IEntityTypeConfiguration<TransactionParty>
{
    public void Configure(EntityTypeBuilder<TransactionParty> builder)
    {
        builder.ToTable("TransactionParties");

        builder.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320);
        builder.Property(x => x.Phone).HasMaxLength(20);

        builder.Property(x => x.PartyType).HasConversion<string>();
        builder.Property(x => x.PartyMode).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => new { x.TransactionId, x.UserId });
        builder.HasIndex(x => x.Email);

        builder.HasOne<Transaction>().WithMany().HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.SetNull);
    }
}
