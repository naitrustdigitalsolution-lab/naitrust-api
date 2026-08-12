using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Application.ExternalServices.QoreId;

public class QoreIdVerificationProvider : IVerificationProvider
{
    private readonly HttpClient _http;
    private readonly QoreIdSettings _settings;
    private readonly ICacheService _cache;
    private readonly ILogger<QoreIdVerificationProvider> _logger;

    private const string TokenCacheKey = "qoreid:access_token";

    public QoreIdVerificationProvider(
        HttpClient http,
        IOptions<QoreIdSettings> settings,
        ICacheService cache,
        ILogger<QoreIdVerificationProvider> logger)
    {
        _http     = http;
        _settings = settings.Value;
        _cache    = cache;
        _logger   = logger;
    }

    // ── BVN ───────────────────────────────────────────────────────────────────

    public async Task<QoreIdBvnResult> VerifyBvnAsync(QoreIdBvnRequest request, CancellationToken ct = default)
    {
        try
        {
            await SetAuthHeaderAsync(ct);

            var url  = $"{BaseUrl}/v1/ng/identities/bvn-basic/{request.BvnNumber}";
            var body = new QoreIdBvnRequestBody(request.FirstName, request.LastName, request.Dob, request.Gender);

            var response = await _http.PostAsync(url, ToJsonContent(body), ct);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("QoreID BVN check failed ({Status}): {Body}", response.StatusCode, err);
                return Fail<QoreIdBvnResult>($"BVN verification failed ({(int)response.StatusCode}).");
            }

            var result = JsonConvert.DeserializeObject<QoreIdBvnApiResponse>(await response.Content.ReadAsStringAsync(ct));
            if (result is null)
            {
                return Fail<QoreIdBvnResult>("Empty response from QoreID.");
            }

            var verified = string.Equals(
                result.Status?.StatusValue, "verified", StringComparison.OrdinalIgnoreCase);

            return new QoreIdBvnResult(
                verified,
                result.Bvn?.Firstname,
                result.Bvn?.Lastname,
                result.Bvn?.Birthdate,
                result.Bvn?.Phone,
                verified ? null : "BVN details do not match our records. Please check and try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "QoreID BVN verification error for BVN ending ...{Tail}",
                request.BvnNumber.Length >= 4 ? request.BvnNumber[^4..] : "****");
            return Fail<QoreIdBvnResult>("Identity verification service is unavailable. Please try again.");
        }
    }

    // ── CAC ───────────────────────────────────────────────────────────────────

    public async Task<QoreIdCacResult> VerifyCacAsync(QoreIdCacRequest request, CancellationToken ct = default)
    {
        try
        {
            await SetAuthHeaderAsync(ct);

            var url  = $"{BaseUrl}/v1/ng/identities/cac-basic";
            var body = new QoreIdCacRequestBody(request.RegNumber);

            var response = await _http.PostAsync(url, ToJsonContent(body), ct);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("QoreID CAC check failed ({Status}): {Body}", response.StatusCode, err);
                return Fail<QoreIdCacResult>($"CAC verification failed ({(int)response.StatusCode}).");
            }

            var result = JsonConvert.DeserializeObject<QoreIdCacApiResponse>(await response.Content.ReadAsStringAsync(ct));
            if (result is null)
            {
                return Fail<QoreIdCacResult>("Empty response from QoreID.");
            }

            var verified = string.Equals(result.Summary?.CacCheck, "verified", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(result.Status?.StatusValue, "verified", StringComparison.OrdinalIgnoreCase);

            return new QoreIdCacResult(
                verified,
                result.Cac?.CompanyName,
                result.Cac?.Status,
                verified ? null : "CAC registration not found or company is not active.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "QoreID CAC verification error for RC {RcNumber}", request.RegNumber);
            return Fail<QoreIdCacResult>("Business verification service is unavailable. Please try again.");
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private string BaseUrl => (_settings.BaseUrl.TrimEnd('/') is { Length: > 0 } url
        ? url
        : "https://api.qoreid.com");

    private async Task SetAuthHeaderAsync(CancellationToken ct)
    {
        var token = await GetAccessTokenAsync(ct);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken ct)
    {
        var cached = await _cache.GetAsync<string>(TokenCacheKey, ct);
        if (cached is not null) { return cached; }

        var tokenUrl = $"{BaseUrl}/token";
        var req      = new QoreIdTokenRequest(_settings.ClientId, _settings.SecretKey);
        var response = await _http.PostAsync(tokenUrl, ToJsonContent(req), ct);

        response.EnsureSuccessStatusCode();

        var tokenData = JsonConvert.DeserializeObject<QoreIdTokenResponse>(await response.Content.ReadAsStringAsync(ct));
        if (tokenData is null || string.IsNullOrEmpty(tokenData.AccessToken))
        {
            throw new InvalidOperationException("QoreID returned an invalid token response.");
        }

        // Token expires in 7200s (2 hrs) — cache for 110 min to be safe
        await _cache.SetAsync(TokenCacheKey, tokenData.AccessToken, TimeSpan.FromMinutes(110), ct);

        _logger.LogInformation("QoreID access token refreshed.");
        return tokenData.AccessToken;
    }

    private static StringContent ToJsonContent(object body) =>
        new(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

    private static T Fail<T>(string message) where T : class
    {
        if (typeof(T) == typeof(QoreIdBvnResult))
        {
            return (new QoreIdBvnResult(false, null, null, null, null, message) as T)!;
        }
        return (new QoreIdCacResult(false, null, null, message) as T)!;
    }
}
