namespace Naitrust.Application.BackgroundJobs;

public class VerificationExpiryJob
{
    public Task ExecuteAsync(CancellationToken ct = default) => Task.CompletedTask;
}
