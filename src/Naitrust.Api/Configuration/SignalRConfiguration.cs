using Naitrust.Api.Hubs;

namespace Naitrust.Api.Configuration;

public static class SignalRConfiguration
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }

    public static WebApplication MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<DealHub>("/hubs/transactions");
        app.MapHub<VerificationHub>("/hubs/verification");
        app.MapHub<NotificationHub>("/hubs/notifications");
        return app;
    }
}
