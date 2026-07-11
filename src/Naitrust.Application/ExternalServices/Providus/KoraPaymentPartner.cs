namespace Naitrust.Application.ExternalServices.Providus;

public class KoraPaymentPartner : IPaymentPartner
{
    // Placeholder for future Kora integration

    public Task<CreateVirtualAccountResult> CreateVirtualAccountAsync(CreateVirtualAccountPartnerRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException("Placeholder for future Kora integration");

    public Task<VerifiedWebhookEvent> VerifyWebhookAsync(VerifyWebhookRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException("Placeholder for future Kora integration");

    public Task<FundingStatusResult> GetFundingStatusAsync(FundingStatusRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException("Placeholder for future Kora integration");

    public Task<PayoutAccountValidationResult> ValidatePayoutAccountAsync(ValidatePayoutAccountPartnerRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException("Placeholder for future Kora integration");

    public Task<PaymentInstructionResult> ReleaseFundsAsync(ReleaseFundsRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException("Placeholder for future Kora integration");

    public Task<PaymentInstructionResult> RefundFundsAsync(RefundFundsRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException("Placeholder for future Kora integration");
}
