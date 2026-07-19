using Hangfire;
using Hangfire.PostgreSql;
using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Api.Configuration;

public static class HangfireConfiguration
{
    public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireSettings = configuration.GetSection("Hangfire").Get<HangfireSettings>();
        var connectionString = hangfireSettings?.ConnectionString ?? configuration.GetConnectionString("NaitrustDbConnection");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options =>
                options.UseNpgsqlConnection(connectionString)));

        services.AddHangfireServer();

        return services;
    }
}
