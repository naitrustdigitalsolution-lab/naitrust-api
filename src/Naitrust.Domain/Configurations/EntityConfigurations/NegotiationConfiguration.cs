using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class NegotiationConfiguration : IEntityTypeConfiguration<Negotiation>
{
    public void Configure(EntityTypeBuilder<Negotiation> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.DealId).HasColumnName("TransactionId").IsRequired();
        builder.Property(n => n.InitiatedByUserId).IsRequired();
        builder.Property(n => n.Status).HasConversion<string>().HasMaxLength(50);
        builder.HasIndex(n => n.DealId);
    }
}
