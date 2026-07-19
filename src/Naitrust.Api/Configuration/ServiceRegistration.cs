using Microsoft.AspNetCore.Identity;
using Naitrust.Application.Configurations;
using Naitrust.Domain.Configurations.ConfigModels;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure;
using Naitrust.Infrastructure.Context;

namespace Naitrust.Api.Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind configuration sections
        services.BindConfiguration(configuration);

        // HttpContextAccessor for audit trail in DbContext
        services.AddHttpContextAccessor();

        // Infrastructure: DbContext, Repository, UoW, Security
        services.AddInfrastructure(configuration);

        // ASP.NET Identity
        services.AddIdentity<NaitrustUser, NaitrustRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<NaitrustDbContext>()
            .AddDefaultTokenProviders();

        // Application: all business services
        services.AddApplicationLayer();

        // API-level services
        services.AddJwtAuthentication(configuration);
        services.AddSwaggerDocumentation();
        services.AddCorsPolicy(configuration);
        services.AddHangfireServices(configuration);
        services.AddSignalRServices();
        services.AddRedisCache(configuration);
        services.AddFluentValidationServices();

        // Exception handler
        services.AddExceptionHandler<Naitrust.Api.Middleware.GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection BindConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.Configure<DatabaseSettings>(configuration.GetSection("ConnectionStrings"));
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        services.Configure<HangfireSettings>(configuration.GetSection("Hangfire"));
        services.Configure<ProvidusSettings>(configuration.GetSection("Providus"));
        services.Configure<QoreIdSettings>(configuration.GetSection("QoreId"));
        services.Configure<StorageSettings>(configuration.GetSection("Storage"));
        services.Configure<EmailSettings>(configuration.GetSection("Email"));
        services.Configure<SmsSettings>(configuration.GetSection("Sms"));
        services.Configure<OpenAiSettings>(configuration.GetSection("OpenAi"));
        services.Configure<CorsSettings>(configuration.GetSection("Cors"));
        services.Configure<AppSettings>(configuration.GetSection("App"));

        return services;
    }
}
