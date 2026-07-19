using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naitrust.Infrastructure.Context;
using Naitrust.Infrastructure.Data.Implementations;
using Naitrust.Infrastructure.Data.Interfaces;
using Naitrust.Infrastructure.Security;

namespace Naitrust.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<NaitrustDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("NaitrustDbConnection"),
                b => b.MigrationsAssembly(typeof(NaitrustDbContext).Assembly.FullName)));

        // Repository & Unit of Work
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork<NaitrustDbContext>>();

        // Security
        services.AddSingleton<IEncryptionHelper, EncryptionHelper>();
        services.AddSingleton<IWebhookSignatureValidator, WebhookSignatureValidator>();

        return services;
    }
}
