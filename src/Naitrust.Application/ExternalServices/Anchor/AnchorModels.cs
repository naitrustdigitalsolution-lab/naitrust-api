using System.Text.Json.Serialization;

namespace Naitrust.Application.ExternalServices.Anchor;

// ── Generic JSON:API envelope ────────────────────────────────────────────────

internal sealed class AnchorRequest<TAttr>
{
    [JsonPropertyName("data")]
    public AnchorRequestData<TAttr> Data { get; set; } = default!;
}

internal sealed class AnchorRequestData<TAttr>
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("attributes")]
    public TAttr Attributes { get; set; } = default!;

    [JsonPropertyName("relationships")]
    public Dictionary<string, AnchorRelationship>? Relationships { get; set; }
}

internal sealed class AnchorResponse<TAttr>
{
    [JsonPropertyName("data")]
    public AnchorResponseData<TAttr> Data { get; set; } = default!;

    [JsonPropertyName("included")]
    public List<AnchorIncluded>? Included { get; set; }
}

internal sealed class AnchorResponseData<TAttr>
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("attributes")]
    public TAttr Attributes { get; set; } = default!;
}

internal sealed class AnchorIncluded
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("attributes")]
    public Dictionary<string, object?>? Attributes { get; set; }
}

internal sealed class AnchorRelationship
{
    [JsonPropertyName("data")]
    public AnchorResourceId Data { get; set; } = default!;
}

internal sealed class AnchorResourceId
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;
}

// ── Customer ─────────────────────────────────────────────────────────────────

internal sealed class AnchorCustomerAttributes
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = default!;

    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = default!;
}

internal sealed class AnchorCustomerResponseAttributes
{
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

// ── Sub-account ───────────────────────────────────────────────────────────────

internal sealed class AnchorSubAccountAttributes
{
    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = "escrow";

    [JsonPropertyName("createVirtualNuban")]
    public bool CreateVirtualNuban { get; set; } = true;
}

internal sealed class AnchorSubAccountResponseAttributes
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("accountName")]
    public string? AccountName { get; set; }

    [JsonPropertyName("balance")]
    public long? Balance { get; set; }
}

// ── Account (balance check) ───────────────────────────────────────────────────

internal sealed class AnchorAccountAttributes
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("balance")]
    public long? Balance { get; set; }
}

// ── NIP Transfer ──────────────────────────────────────────────────────────────

internal sealed class AnchorNipTransferAttributes
{
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = default!;

    [JsonPropertyName("beneficiaryAccountNumber")]
    public string BeneficiaryAccountNumber { get; set; } = default!;

    [JsonPropertyName("beneficiaryBankCode")]
    public string BeneficiaryBankCode { get; set; } = default!;
}

internal sealed class AnchorTransferResponseAttributes
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("amount")]
    public long? Amount { get; set; }

    [JsonPropertyName("narration")]
    public string? Narration { get; set; }
}

// ── Internal Transfer (subledger-to-subledger book transfer) ──────────────────

internal sealed class AnchorAccountTransferAttributes
{
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = default!;
}

// ── Name enquiry ──────────────────────────────────────────────────────────────

internal sealed class AnchorNameEnquiryAttributes
{
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = default!;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = default!;
}

internal sealed class AnchorNameEnquiryResponseAttributes
{
    [JsonPropertyName("accountName")]
    public string? AccountName { get; set; }

    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }
}

// ── Customer creation request (public — used by SecurityService / KYC) ────────

public record AnchorCreateCustomerRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    string Bvn);
