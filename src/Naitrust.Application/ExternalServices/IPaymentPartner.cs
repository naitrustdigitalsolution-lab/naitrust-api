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

public record CreateVirtualAccountPartnerRequest(Guid TransactionId, long AmountMinor, string Currency, string AccountName);
public record CreateVirtualAccountResult(string ProviderReference, string AccountNumber, string AccountName, string BankName, DateTime? ExpiresAt);
public record VerifyWebhookRequest(string Payload, string Signature);
public record VerifiedWebhookEvent(string EventId, string EventType, string VirtualAccountReference, long AmountMinor, string Currency, string? PaidByMetadata, string RawPayload);
public record FundingStatusRequest(string ProviderReference);
public record FundingStatusResult(string Status, long AmountReceivedMinor);
public record ValidatePayoutAccountPartnerRequest(string BankCode, string AccountNumber);
public record PayoutAccountValidationResult(string AccountName, string AccountNumber, string BankName);
public record ReleaseFundsRequest(Guid TransactionId, string IdempotencyKey, long AmountMinor, string Currency, string DestinationAccountNumber, string DestinationBankCode);
public record RefundFundsRequest(Guid TransactionId, string IdempotencyKey, long AmountMinor, string Currency, string DestinationAccountNumber, string DestinationBankCode);
public record PaymentInstructionResult(string PartnerReference, string Status);
