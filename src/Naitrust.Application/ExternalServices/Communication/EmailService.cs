using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Naitrust.Domain.Configurations.ConfigModels;
using Naitrust.Domain.Models.Enums.Communication;

namespace Naitrust.Application.ExternalServices.Communication;

public class EmailService : IEmailService
{
    private readonly IEmailProviderFactory _providerFactory;
    private readonly EmailSettings _settings;
    private readonly AppSettings _appSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IEmailProviderFactory providerFactory,
        IOptions<EmailSettings> settings,
        IOptions<AppSettings> appSettings,
        ILogger<EmailService> logger)
    {
        _providerFactory = providerFactory;
        _settings = settings.Value;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody, string textBody, CancellationToken ct = default)
    {
        var provider = GetProvider();
        var message = new EmailMessage(to, subject, htmlBody, textBody);
        await provider.SendAsync(message, ct);
    }

    public async Task SendVerificationOtpAsync(string to, string firstName, string email, string otp, CancellationToken ct = default)
    {
        var webBaseUrl = _appSettings.WebBaseUrl;
        var verificationLink = $"{webBaseUrl}/verify-email?email={HttpUtility.UrlEncode(email)}&otp={otp}";

        var content = EmailTemplateHelper.BuildOtpContent(
            firstName,
            "Welcome to Naitrust. Verify your email address using the code below:",
            otp);

        var html = EmailTemplateHelper.GenerateEmailHtml(
            "Verify Your Email",
            content,
            webBaseUrl,
            buttonText: "Verify Email Address",
            buttonUrl: verificationLink);

        var text = $"Hello {firstName},\n\nYour verification code is: {otp}\n\nOr verify here: {verificationLink}\n\nExpires in 15 minutes.\n\n-- Naitrust Digital Solutions Limited";

        var provider = GetProvider();
        await provider.SendAsync(new EmailMessage(to, $"{otp} is your Naitrust verification code", html, text), ct);
    }

    public async Task SendPasswordResetOtpAsync(string to, string userName, string otp, CancellationToken ct = default)
    {
        var webBaseUrl = _appSettings.WebBaseUrl;

        var content = EmailTemplateHelper.BuildOtpContent(
            userName,
            "We received a request to reset your password. Use the code below:",
            otp);

        var html = EmailTemplateHelper.GenerateEmailHtml(
            "Reset Your Password",
            content,
            webBaseUrl);

        var text = $"Hello {userName},\n\nYour password reset code is: {otp}\n\nExpires in 15 minutes.\n\n-- Naitrust Digital Solutions Limited";

        var provider = GetProvider();
        await provider.SendAsync(new EmailMessage(to, $"{otp} is your Naitrust password reset code", html, text), ct);
    }

    public async Task SendWelcomeEmailAsync(string to, string firstName, string role, CancellationToken ct = default)
    {
        var webBaseUrl = _appSettings.WebBaseUrl;
        var dashboardUrl = $"{webBaseUrl}/dashboard";

        var content = EmailTemplateHelper.BuildWelcomeContent(firstName);

        var html = EmailTemplateHelper.GenerateEmailHtml(
            "Welcome to Naitrust",
            content,
            webBaseUrl,
            buttonText: "Go to Dashboard",
            buttonUrl: dashboardUrl);

        var text = $"Hello {firstName},\n\nYour email has been verified. Visit your dashboard: {dashboardUrl}\n\n-- Naitrust Digital Solutions Limited";

        var provider = GetProvider();
        await provider.SendAsync(new EmailMessage(to, $"Welcome to Naitrust, {firstName}", html, text), ct);
    }

    private IEmailProvider GetProvider()
    {
        if (!Enum.TryParse<EmailProviderId>(_settings.Provider, true, out var providerId))
        {
            _logger.LogWarning("Unknown email provider '{Provider}', defaulting to Resend", _settings.Provider);
            providerId = EmailProviderId.Resend;
        }

        return _providerFactory.GetProvider(providerId);
    }
}
