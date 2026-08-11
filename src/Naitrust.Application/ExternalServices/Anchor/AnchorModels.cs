using Newtonsoft.Json;

namespace Naitrust.Application.ExternalServices.Anchor;

// ── Generic JSON:API envelope ────────────────────────────────────────────────

internal sealed class AnchorRequest<TAttr>
{
    [JsonProperty("data")]
    public AnchorRequestData<TAttr> Data { get; set; } = default!;
}

internal sealed class AnchorRequestData<TAttr>
{
    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("attributes")]
    public TAttr Attributes { get; set; } = default!;

    [JsonProperty("relationships")]
    public Dictionary<string, AnchorRelationship>? Relationships { get; set; }
}

internal sealed class AnchorResponse<TAttr>
{
    [JsonProperty("data")]
    public AnchorResponseData<TAttr> Data { get; set; } = default!;

    [JsonProperty("included")]
    public List<AnchorIncluded>? Included { get; set; }
}

internal sealed class AnchorResponseData<TAttr>
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("attributes")]
    public TAttr Attributes { get; set; } = default!;
}

internal sealed class AnchorIncluded
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("attributes")]
    public Dictionary<string, object?>? Attributes { get; set; }
}

internal sealed class AnchorRelationship
{
    [JsonProperty("data")]
    public AnchorResourceId Data { get; set; } = default!;
}

internal sealed class AnchorResourceId
{
    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("id")]
    public string Id { get; set; } = default!;
}

// ── Customer ─────────────────────────────────────────────────────────────────

internal sealed class AnchorCustomerAttributes
{
    [JsonProperty("fullName")]
    public string FullName { get; set; } = default!;

    [JsonProperty("email")]
    public string Email { get; set; } = default!;

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = default!;

    [JsonProperty("bvn")]
    public string Bvn { get; set; } = default!;
}

internal sealed class AnchorCustomerResponseAttributes
{
    [JsonProperty("fullName")]
    public string? FullName { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }
}

// ── Sub-account ───────────────────────────────────────────────────────────────

internal sealed class AnchorSubAccountAttributes
{
    [JsonProperty("productName")]
    public string ProductName { get; set; } = "escrow";

    [JsonProperty("createVirtualNuban")]
    public bool CreateVirtualNuban { get; set; } = true;
}

internal sealed class AnchorSubAccountResponseAttributes
{
    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("accountName")]
    public string? AccountName { get; set; }

    [JsonProperty("balance")]
    public long? Balance { get; set; }
}

// ── Account (balance check) ───────────────────────────────────────────────────

internal sealed class AnchorAccountAttributes
{
    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("balance")]
    public long? Balance { get; set; }
}

// ── NIP Transfer ──────────────────────────────────────────────────────────────

internal sealed class AnchorNipTransferAttributes
{
    [JsonProperty("amount")]
    public long Amount { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; } = "NGN";

    [JsonProperty("narration")]
    public string Narration { get; set; } = default!;

    [JsonProperty("beneficiaryAccountNumber")]
    public string BeneficiaryAccountNumber { get; set; } = default!;

    [JsonProperty("beneficiaryBankCode")]
    public string BeneficiaryBankCode { get; set; } = default!;
}

internal sealed class AnchorTransferResponseAttributes
{
    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("amount")]
    public long? Amount { get; set; }

    [JsonProperty("narration")]
    public string? Narration { get; set; }
}

// ── Internal Transfer (subledger-to-subledger book transfer) ──────────────────

internal sealed class AnchorAccountTransferAttributes
{
    [JsonProperty("amount")]
    public long Amount { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; } = "NGN";

    [JsonProperty("narration")]
    public string Narration { get; set; } = default!;
}

// ── Name enquiry ──────────────────────────────────────────────────────────────

internal sealed class AnchorNameEnquiryAttributes
{
    [JsonProperty("bankCode")]
    public string BankCode { get; set; } = default!;

    [JsonProperty("accountNumber")]
    public string AccountNumber { get; set; } = default!;
}

internal sealed class AnchorNameEnquiryResponseAttributes
{
    [JsonProperty("accountName")]
    public string? AccountName { get; set; }

    [JsonProperty("bankName")]
    public string? BankName { get; set; }

    [JsonProperty("accountNumber")]
    public string? AccountNumber { get; set; }
}

// ── Customer creation request (public — used by SecurityService / KYC) ────────

public record AnchorCreateCustomerRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    string Bvn);
