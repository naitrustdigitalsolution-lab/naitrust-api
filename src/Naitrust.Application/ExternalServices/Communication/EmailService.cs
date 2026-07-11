namespace Naitrust.Application.ExternalServices.Communication;

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task SendTemplatedEmailAsync(string to, string templateName, object data, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
