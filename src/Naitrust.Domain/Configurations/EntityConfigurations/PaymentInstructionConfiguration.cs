using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class PaymentInstructionConfiguration : IEntityTypeConfiguration<PaymentInstruction>
{
    public void Configure(EntityTypeBuilder<PaymentInstruction> builder)
    {
        builder.ToTable("PaymentInstructions");

        builder.Property(x => x.IdempotencyKey).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SignedPayloadHash).HasMaxLength(512);
        builder.Property(x => x.PartnerReference).HasMaxLength(200);

        builder.Property(x => x.InstructionType).HasConversion<string>();
        builder.Property(x => x.Partner).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
        builder.HasIndex(x => x.TransactionId);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<VirtualAccount>().WithMany().HasForeignKey(x => x.VirtualAccountId).OnDelete(DeleteBehavior.Restrict);
    }
}
