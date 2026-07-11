using Microsoft.EntityFrameworkCore;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Infrastructure.Data.Extension;

public static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
    }

    public static void ApplyDecimalPrecision(this ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }

    public static void ApplyDefaultStringLength(this ModelBuilder modelBuilder, int maxLength = 256)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(string) && p.GetMaxLength() == null))
        {
            property.SetMaxLength(maxLength);
        }
    }
}
