using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Payments;
using Naitrust.Domain.Models.Dtos.Responses.Payments;

namespace Naitrust.Application.Services.Implementations.Payments;

public class PaymentService : IPaymentService
{
    public Task<NaitrustResponse<VirtualAccountResponse>> CreateVirtualAccountAsync(CreateVirtualAccountRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaymentStatusResponse>> GetPaymentStatusAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<ReleaseRequestResponse>> RequestReleaseAsync(Guid transactionId, RequestReleaseRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<LedgerEntryResponse>>> GetLedgerAsync(Guid transactionId, PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<ReconciliationStatusResponse>> GetReconciliationStatusAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PayoutAccountValidationResponse>> ValidatePayoutAccountAsync(ValidatePayoutAccountRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
