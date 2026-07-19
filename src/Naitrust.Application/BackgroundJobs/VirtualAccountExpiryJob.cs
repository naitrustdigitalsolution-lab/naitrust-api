namespace Naitrust.Application.BackgroundJobs;

public class VirtualAccountExpiryJob
{
    public Task ExecuteAsync(CancellationToken ct = default) => Task.CompletedTask;
}
