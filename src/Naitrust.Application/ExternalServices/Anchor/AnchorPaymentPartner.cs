using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Application.ExternalServices.Anchor;

public class AnchorPaymentPartner : IPaymentPartner
{
    private readonly HttpClient _httpClient;
    private readonly AnchorSettings _settings;
    private readonly ILogger<AnchorPaymentPartner> _logger;

    public AnchorPaymentPartner(
        HttpClient httpClient,
        IOptions<AnchorSettings> settings,
        ILogger<AnchorPaymentPartner> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl.TrimEnd('/') + "/");
        _httpClient.DefaultRequestHeaders.Add("x-anchor-key", _settings.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    // ── Create Virtual Account (per-deal escrow sub-account) ──────────────────

    /// <summary>
    /// Creates a per-deal Anchor sub-account with a dedicated virtual NUBAN.
    /// Requires request.CustomerReference = the buyer's AnchorCustomerId.
    /// Anchor mandates: customer must be created first before any sub-account.
    /// </summary>
    public async Task<CreateVirtualAccountResult> CreateVirtualAccountAsync(
        CreateVirtualAccountPartnerRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(request.CustomerReference))
            throw new InvalidOperationException(
                "Anchor requires a customer ID (CustomerReference). " +
                "Ensure CreateCustomerAsync was called during KYC and AnchorCustomerId is stored on the user.");

        var body = new AnchorRequest<AnchorSubAccountAttributes>
        {
            Data = new AnchorRequestData<AnchorSubAccountAttributes>
            {
                Type = "DepositAccount",
                Attributes = new AnchorSubAccountAttributes
                {
                    ProductName = "escrow",
                    CreateVirtualNuban = true
                },
                Relationships = new Dictionary<string, AnchorRelationship>
                {
                    ["customer"] = new AnchorRelationship
                    {
                        Data = new AnchorResourceId { Type = "Customer", Id = request.CustomerReference }
                    }
                }
            }
        };

        var response = await PostAsync<AnchorResponse<AnchorSubAccountResponseAttributes>>(
            "sub-accounts", body, ct);

        var subAccountId = response.Data.Id;

        // Anchor returns the virtual NUBAN in the `included` array as type "VirtualNuban"
        var nubanItem = response.Included?
            .FirstOrDefault(x => x.Type is "VirtualNuban" or "virtualNuban");

        var accountNumber = nubanItem?.Attributes?
            .GetValueOrDefault("accountNumber")?.ToString() ?? "";

        var bankName = nubanItem?.Attributes?
            .GetValueOrDefault("bankName")?.ToString() ?? "Providus Bank";

        _logger.LogInformation(
            "Anchor sub-account created for deal {DealId}: SubAccountId={SubAccountId}, NUBAN={NUBAN}",
            request.TransactionId, subAccountId, accountNumber);

        return new CreateVirtualAccountResult(
            ProviderReference: subAccountId,
            AccountNumber: accountNumber,
            AccountName: request.AccountName,
            BankName: bankName,
            ExpiresAt: null);
    }

    // ── Verify Webhook ────────────────────────────────────────────────────────

    public Task<VerifiedWebhookEvent> VerifyWebhookAsync(VerifyWebhookRequest request, CancellationToken ct = default)
    {
        if (!VerifySignature(request.Payload, request.Signature))
            throw new UnauthorizedAccessException("Anchor webhook signature verification failed.");

        var root = JObject.Parse(request.Payload);

        var eventType = root["event"]?.Value<string>() ?? "";

        var data = root["data"] as JObject ?? new JObject();
        var eventId = data["id"]?.Value<string>() ?? Guid.NewGuid().ToString();

        var amountMinor = data["attributes"]?["amount"]?.Value<long>() ?? 0L;
        var currency = data["attributes"]?["currency"]?.Value<string>() ?? "NGN";

        // Sub-account reference lives in relationships.account.data.id
        var virtualAccountRef = data["relationships"]?["account"]?["data"]?["id"]?.Value<string>() ?? "";

        _logger.LogInformation(
            "Anchor webhook received: Event={Event}, SubAccount={SubAccount}, Amount={Amount}",
            eventType, virtualAccountRef, amountMinor);

        return Task.FromResult(new VerifiedWebhookEvent(
            EventId: eventId,
            EventType: eventType,
            VirtualAccountReference: virtualAccountRef,
            AmountMinor: amountMinor,
            Currency: currency,
            PaidByMetadata: null,
            RawPayload: request.Payload));
    }

    // ── Get Funding Status ────────────────────────────────────────────────────

    public virtual async Task<FundingStatusResult> GetFundingStatusAsync(
        FundingStatusRequest request, CancellationToken ct = default)
    {
        var httpResp = await _httpClient.GetAsync($"accounts/{request.ProviderReference}", ct);
        var responseBody = await httpResp.Content.ReadAsStringAsync(ct);
        var response = JsonConvert.DeserializeObject<AnchorResponse<AnchorAccountAttributes>>(responseBody);

        if (response is null) { return new FundingStatusResult("unknown", 0); }

        var attrs = response.Data.Attributes;
        return new FundingStatusResult(
            Status: attrs.Status ?? "active",
            AmountReceivedMinor: attrs.Balance ?? 0);
    }

    // ── Validate Payout Account (name enquiry) ────────────────────────────────

    public async Task<PayoutAccountValidationResult> ValidatePayoutAccountAsync(
        ValidatePayoutAccountPartnerRequest request, CancellationToken ct = default)
    {
        var body = new AnchorRequest<AnchorNameEnquiryAttributes>
        {
            Data = new AnchorRequestData<AnchorNameEnquiryAttributes>
            {
                Type = "NameEnquiry",
                Attributes = new AnchorNameEnquiryAttributes
                {
                    BankCode = request.BankCode,
                    AccountNumber = request.AccountNumber
                }
            }
        };

        var response = await PostAsync<AnchorResponse<AnchorNameEnquiryResponseAttributes>>(
            "transfers/name-enquiry", body, ct);

        var attrs = response.Data.Attributes;
        return new PayoutAccountValidationResult(
            AccountName: attrs.AccountName ?? "",
            AccountNumber: request.AccountNumber,
            BankName: attrs.BankName ?? "");
    }

    // ── Release / Refund Funds ────────────────────────────────────────────────

    public Task<PaymentInstructionResult> ReleaseFundsAsync(
        ReleaseFundsRequest request, CancellationToken ct = default) =>
        NipTransferAsync(
            idempotencyKey: request.IdempotencyKey,
            sourceSubAccountId: request.SourceAccountReference,
            amountMinor: request.AmountMinor,
            currency: request.Currency,
            destinationAccountNumber: request.DestinationAccountNumber,
            destinationBankCode: request.DestinationBankCode,
            narration: $"Deal payout – {request.TransactionId}",
            ct);

    public Task<PaymentInstructionResult> RefundFundsAsync(
        RefundFundsRequest request, CancellationToken ct = default) =>
        NipTransferAsync(
            idempotencyKey: request.IdempotencyKey,
            sourceSubAccountId: request.SourceAccountReference,
            amountMinor: request.AmountMinor,
            currency: request.Currency,
            destinationAccountNumber: request.DestinationAccountNumber,
            destinationBankCode: request.DestinationBankCode,
            narration: $"Deal refund – {request.TransactionId}",
            ct);

    // ── Internal subledger-to-subledger transfer ──────────────────────────────

    /// <summary>
    /// Transfers funds between two Anchor subledgers (e.g. buyer wallet → platform escrow).
    /// This is an instant book transfer with no NIP fees.
    /// </summary>
    public virtual async Task<PaymentInstructionResult> InternalTransferAsync(
        string sourceSubAccountId,
        string destinationSubAccountId,
        long amountMinor,
        string currency,
        string narration,
        string idempotencyKey,
        CancellationToken ct = default)
    {
        var body = new AnchorRequest<AnchorAccountTransferAttributes>
        {
            Data = new AnchorRequestData<AnchorAccountTransferAttributes>
            {
                Type = "InternalTransfer",
                Attributes = new AnchorAccountTransferAttributes
                {
                    Amount = amountMinor,
                    Currency = currency,
                    Narration = narration
                },
                Relationships = new Dictionary<string, AnchorRelationship>
                {
                    ["sourceAccount"] = new AnchorRelationship
                    {
                        Data = new AnchorResourceId { Type = "SubAccount", Id = sourceSubAccountId }
                    },
                    ["destinationAccount"] = new AnchorRelationship
                    {
                        Data = new AnchorResourceId { Type = "SubAccount", Id = destinationSubAccountId }
                    }
                }
            }
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, "transfers")
        {
            Content = ToJsonContent(body)
        };
        req.Headers.Add("x-idempotency-key", idempotencyKey);

        var httpResp = await _httpClient.SendAsync(req, ct);

        if (!httpResp.IsSuccessStatusCode)
        {
            var err = await httpResp.Content.ReadAsStringAsync(ct);
            _logger.LogError("Anchor internal transfer failed {Status}: {Body}", httpResp.StatusCode, err);
            throw new InvalidOperationException($"Anchor internal transfer failed ({httpResp.StatusCode}): {err}");
        }

        var resultJson = await httpResp.Content.ReadAsStringAsync(ct);
        var result = JsonConvert.DeserializeObject<AnchorResponse<AnchorTransferResponseAttributes>>(resultJson);

        _logger.LogInformation(
            "Anchor internal transfer {Id} from {Src} to {Dst}: {Amount} {Currency}",
            result!.Data.Id, sourceSubAccountId, destinationSubAccountId, amountMinor, currency);

        return new PaymentInstructionResult(
            PartnerReference: result.Data.Id,
            Status: result.Data.Attributes.Status ?? "pending");
    }

    // ── User withdrawal (subledger → external bank via NIP) ──────────────────

    /// <summary>
    /// Initiates a NIP transfer from the user's subledger to their personal bank account.
    /// </summary>
    public Task<PaymentInstructionResult> WithdrawToExternalBankAsync(
        string sourceSubAccountId,
        long amountMinor,
        string currency,
        string destinationAccountNumber,
        string destinationBankCode,
        string narration,
        string idempotencyKey,
        CancellationToken ct = default) =>
        NipTransferAsync(
            idempotencyKey: idempotencyKey,
            sourceSubAccountId: sourceSubAccountId,
            amountMinor: amountMinor,
            currency: currency,
            destinationAccountNumber: destinationAccountNumber,
            destinationBankCode: destinationBankCode,
            narration: narration,
            ct: ct);

    // ── Customer onboarding (called once during KYC) ──────────────────────────

    /// <summary>
    /// Creates an Anchor IndividualCustomer and returns the Anchor customer ID.
    /// Call this when user submits BVN during KYC. Store result in NaitrustUser.AnchorCustomerId.
    /// This MUST happen before CreateVirtualAccountAsync can be called for any deal.
    /// </summary>
    public async Task<string> CreateCustomerAsync(AnchorCreateCustomerRequest request, CancellationToken ct = default)
    {
        var body = new AnchorRequest<AnchorCustomerAttributes>
        {
            Data = new AnchorRequestData<AnchorCustomerAttributes>
            {
                Type = "IndividualCustomer",
                Attributes = new AnchorCustomerAttributes
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Bvn = request.Bvn
                }
            }
        };

        var response = await PostAsync<AnchorResponse<AnchorCustomerResponseAttributes>>("customers", body, ct);

        _logger.LogInformation("Anchor customer created: {CustomerId} for {Email}",
            response.Data.Id, request.Email);

        return response.Data.Id;
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<PaymentInstructionResult> NipTransferAsync(
        string idempotencyKey,
        string sourceSubAccountId,
        long amountMinor,
        string currency,
        string destinationAccountNumber,
        string destinationBankCode,
        string narration,
        CancellationToken ct)
    {
        var body = new AnchorRequest<AnchorNipTransferAttributes>
        {
            Data = new AnchorRequestData<AnchorNipTransferAttributes>
            {
                Type = "NIPTransfer",
                Attributes = new AnchorNipTransferAttributes
                {
                    Amount = amountMinor,
                    Currency = currency,
                    Narration = narration,
                    BeneficiaryAccountNumber = destinationAccountNumber,
                    BeneficiaryBankCode = destinationBankCode
                },
                Relationships = new Dictionary<string, AnchorRelationship>
                {
                    ["account"] = new AnchorRelationship
                    {
                        Data = new AnchorResourceId { Type = "SubAccount", Id = sourceSubAccountId }
                    }
                }
            }
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, "transfers")
        {
            Content = ToJsonContent(body)
        };
        req.Headers.Add("x-idempotency-key", idempotencyKey);

        var httpResp = await _httpClient.SendAsync(req, ct);

        if (!httpResp.IsSuccessStatusCode)
        {
            var err = await httpResp.Content.ReadAsStringAsync(ct);
            _logger.LogError("Anchor NIP transfer failed {Status}: {Body}", httpResp.StatusCode, err);
            throw new InvalidOperationException($"Anchor transfer failed ({httpResp.StatusCode}): {err}");
        }

        var resultJson = await httpResp.Content.ReadAsStringAsync(ct);
        var result = JsonConvert.DeserializeObject<AnchorResponse<AnchorTransferResponseAttributes>>(resultJson);

        return new PaymentInstructionResult(
            PartnerReference: result!.Data.Id,
            Status: result.Data.Attributes.Status ?? "pending");
    }

    private async Task<TResp> PostAsync<TResp>(string path, object body, CancellationToken ct)
    {
        var httpResp = await _httpClient.PostAsync(path, ToJsonContent(body), ct);

        if (!httpResp.IsSuccessStatusCode)
        {
            var err = await httpResp.Content.ReadAsStringAsync(ct);
            _logger.LogError("Anchor API POST {Path} failed {Status}: {Body}", path, httpResp.StatusCode, err);
            throw new InvalidOperationException($"Anchor API error ({httpResp.StatusCode}): {err}");
        }

        var json = await httpResp.Content.ReadAsStringAsync(ct);
        return JsonConvert.DeserializeObject<TResp>(json)!;
    }

    private static StringContent ToJsonContent(object body) =>
        new(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

    private bool VerifySignature(string payload, string signature)
    {
        if (string.IsNullOrEmpty(_settings.WebhookSecret)) return true; // dev bypass

        var key = Encoding.UTF8.GetBytes(_settings.WebhookSecret);
        var data = Encoding.UTF8.GetBytes(payload);
        var computed = Convert.ToHexString(HMACSHA256.HashData(key, data)).ToLowerInvariant();
        return computed == signature.ToLowerInvariant();
    }
}
