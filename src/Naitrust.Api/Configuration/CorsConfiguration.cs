using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Api.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection("Cors").Get<CorsSettings>();

        services.AddCors(options =>
        {
            options.AddPolicy("NaitrustCorsPolicy", builder =>
            {
                builder
                    .WithOrigins(corsSettings?.AllowedOrigins ?? ["http://localhost:3000"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
