namespace Naitrust.Application.ExternalServices;

public interface IPaymentPartner
{
    Task<CreateVirtualAccountResult> CreateVirtualAccountAsync(CreateVirtualAccountPartnerRequest request, CancellationToken ct = default);
    Task<VerifiedWebhookEvent> VerifyWebhookAsync(VerifyWebhookRequest request, CancellationToken ct = default);
    Task<FundingStatusResult> GetFundingStatusAsync(FundingStatusRequest request, CancellationToken ct = default);
    Task<PayoutAccountValidationResult> ValidatePayoutAccountAsync(ValidatePayoutAccountPartnerRequest request, CancellationToken ct = default);
    Task<PaymentInstructionResult> ReleaseFundsAsync(ReleaseFundsRequest request, CancellationToken ct = default);
    Task<PaymentInstructionResult> RefundFundsAsync(RefundFundsRequest request, CancellationToken ct = default);
}

/// <summary>
/// CustomerReference is required by Anchor (the buyer's AnchorCustomerId).
/// Other partners may leave it null.
/// </summary>
public record CreateVirtualAccountPartnerRequest(
    Guid TransactionId,
    long AmountMinor,
    string Currency,
    string AccountName,
    string? CustomerReference = null);

public record CreateVirtualAccountResult(
    string ProviderReference,
    string AccountNumber,
    string AccountName,
    string BankName,
    DateTime? ExpiresAt);

public record VerifyWebhookRequest(string Payload, string Signature);

public record VerifiedWebhookEvent(
    string EventId,
    string EventType,
    string VirtualAccountReference,
    long AmountMinor,
    string Currency,
    string? PaidByMetadata,
    string RawPayload);

public record FundingStatusRequest(string ProviderReference);

public record FundingStatusResult(string Status, long AmountReceivedMinor);

public record ValidatePayoutAccountPartnerRequest(string BankCode, string AccountNumber);

public record PayoutAccountValidationResult(string AccountName, string AccountNumber, string BankName);

/// <summary>
/// SourceAccountReference is the partner's sub-account/wallet ID to debit.
/// Required by Anchor; other partners may derive it differently.
/// </summary>
public record ReleaseFundsRequest(
    Guid TransactionId,
    string IdempotencyKey,
    string SourceAccountReference,
    long AmountMinor,
    string Currency,
    string DestinationAccountNumber,
    string DestinationBankCode);

public record RefundFundsRequest(
    Guid TransactionId,
    string IdempotencyKey,
    string SourceAccountReference,
    long AmountMinor,
    string Currency,
    string DestinationAccountNumber,
    string DestinationBankCode);

public record PaymentInstructionResult(string PartnerReference, string Status);
