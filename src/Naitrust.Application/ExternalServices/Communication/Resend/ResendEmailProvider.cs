using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Application.ExternalServices.Communication.Resend;

public class ResendEmailProvider : IEmailProvider
{
    private readonly HttpClient _httpClient;
    private readonly EmailSettings _settings;
    private readonly ILogger<ResendEmailProvider> _logger;

    public ResendEmailProvider(
        HttpClient httpClient,
        IOptions<EmailSettings> settings,
        ILogger<ResendEmailProvider> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri("https://api.resend.com/");
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
        var from = message.From ?? $"{_settings.FromName} <{_settings.FromAddress}>";
        var replyTo = message.ReplyTo ?? _settings.ReplyToAddress;

        var payload = new ResendRequest
        {
            From = from,
            To = [message.To],
            Subject = message.Subject,
            Html = message.HtmlBody,
            Text = message.TextBody,
            ReplyTo = replyTo
        };

        var response = await _httpClient.PostAsJsonAsync("emails", payload, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError("Resend API error {StatusCode}: {Body}", response.StatusCode, errorBody);
            throw new InvalidOperationException($"Resend email failed ({response.StatusCode}): {errorBody}");
        }

        _logger.LogInformation("Email sent via Resend to {To}, subject: {Subject}", message.To, message.Subject);
    }

    private sealed class ResendRequest
    {
        [JsonPropertyName("from")]
        public string From { get; set; } = "";

        [JsonPropertyName("to")]
        public string[] To { get; set; } = [];

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = "";

        [JsonPropertyName("html")]
        public string Html { get; set; } = "";

        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("reply_to")]
        public string? ReplyTo { get; set; }
    }
}
