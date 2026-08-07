namespace Naitrust.Application.ExternalServices;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody, string textBody, CancellationToken ct = default);
    Task SendVerificationOtpAsync(string to, string firstName, string email, string otp, CancellationToken ct = default);
    Task SendPasswordResetOtpAsync(string to, string userName, string otp, CancellationToken ct = default);
    Task SendWelcomeEmailAsync(string to, string firstName, string role, CancellationToken ct = default);
}
