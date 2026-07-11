namespace Naitrust.Application.ExternalServices.Providus;

public class ProvidusPaymentPartner : IPaymentPartner
{
    public Task<CreateVirtualAccountResult> CreateVirtualAccountAsync(CreateVirtualAccountPartnerRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<VerifiedWebhookEvent> VerifyWebhookAsync(VerifyWebhookRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<FundingStatusResult> GetFundingStatusAsync(FundingStatusRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<PayoutAccountValidationResult> ValidatePayoutAccountAsync(ValidatePayoutAccountPartnerRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<PaymentInstructionResult> ReleaseFundsAsync(ReleaseFundsRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<PaymentInstructionResult> RefundFundsAsync(RefundFundsRequest request, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
