namespace Naitrust.Application.ExternalServices;

public interface IEmailProvider
{
    Task SendAsync(EmailMessage message, CancellationToken ct = default);
}

public record EmailMessage(
    string To,
    string Subject,
    string HtmlBody,
    string TextBody,
    string? From = null,
    string? ReplyTo = null);
