namespace Naitrust.Application.ExternalServices;

public interface ISmsService
{
    Task SendSmsAsync(string to, string message, CancellationToken ct = default);
    Task SendOtpAsync(string to, CancellationToken ct = default);
}
