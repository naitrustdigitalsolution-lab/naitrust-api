namespace Naitrust.Application.BackgroundJobs;

public class NotificationDispatchJob
{
    public Task ExecuteAsync(CancellationToken ct = default) => Task.CompletedTask;
}
