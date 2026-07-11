using Microsoft.Extensions.Hosting;

namespace Naitrust.Infrastructure.BackgroundServices;

public class OutboxBackgroundService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}
