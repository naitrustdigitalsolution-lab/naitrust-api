using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class NegotiationProposalConfiguration : IEntityTypeConfiguration<NegotiationProposal>
{
    public void Configure(EntityTypeBuilder<NegotiationProposal> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.NegotiationId).IsRequired();
        builder.Property(p => p.ProposedByUserId).IsRequired();
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50);
        builder.HasIndex(p => p.NegotiationId);
    }
}
