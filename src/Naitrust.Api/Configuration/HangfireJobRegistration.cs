using Hangfire;
using Naitrust.Application.BackgroundJobs;

namespace Naitrust.Api.Configuration;

public static class HangfireJobRegistration
{
    public static void RegisterAll(IServiceProvider services)
    {
        RecurringJob.AddOrUpdate<ReconciliationJob>(
            "reconciliation",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly);

        RecurringJob.AddOrUpdate<AutoConfirmJob>(
            "auto-confirm",
            job => job.ExecuteAsync(CancellationToken.None),
            "*/15 * * * *");

        RecurringJob.AddOrUpdate<NotificationDispatchJob>(
            "notification-dispatch",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Minutely);

        RecurringJob.AddOrUpdate<OutboxProcessorJob>(
            "outbox-processor",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Minutely);

        RecurringJob.AddOrUpdate<WebhookRetryJob>(
            "webhook-retry",
            job => job.ExecuteAsync(CancellationToken.None),
            "*/5 * * * *");

        RecurringJob.AddOrUpdate<VirtualAccountExpiryJob>(
            "virtual-account-expiry",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly);

        RecurringJob.AddOrUpdate<VerificationExpiryJob>(
            "verification-expiry",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly);
    }
}
