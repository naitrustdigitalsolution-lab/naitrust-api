using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Api.Configuration;

public static class RedisConfiguration
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection("Redis").Get<RedisSettings>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings?.ConnectionString;
            options.InstanceName = redisSettings?.InstanceName ?? "naitrust_";
        });

        return services;
    }
}
