namespace Naitrust.Application.ExternalServices;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default);
    Task SendTemplatedEmailAsync(string to, string templateName, object data, CancellationToken ct = default);
}
