using Hangfire;
using Naitrust.Application.BackgroundJobs;

namespace Naitrust.Api.Configuration;

public static class HangfireJobRegistration
{
    public static void RegisterAll(IServiceProvider services)
    {
        var jobManager = services.GetRequiredService<IRecurringJobManager>();

        jobManager.AddOrUpdate<ReconciliationJob>(
            "reconciliation",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly);

        jobManager.AddOrUpdate<AutoConfirmJob>(
            "auto-confirm",
            job => job.ExecuteAsync(CancellationToken.None),
            "*/15 * * * *");

        jobManager.AddOrUpdate<NotificationDispatchJob>(
            "notification-dispatch",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Minutely);

        jobManager.AddOrUpdate<OutboxProcessorJob>(
            "outbox-processor",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Minutely);

        jobManager.AddOrUpdate<WebhookRetryJob>(
            "webhook-retry",
            job => job.ExecuteAsync(CancellationToken.None),
            "*/5 * * * *");

        jobManager.AddOrUpdate<VirtualAccountExpiryJob>(
            "virtual-account-expiry",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly);

        jobManager.AddOrUpdate<VerificationExpiryJob>(
            "verification-expiry",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly);
    }
}
