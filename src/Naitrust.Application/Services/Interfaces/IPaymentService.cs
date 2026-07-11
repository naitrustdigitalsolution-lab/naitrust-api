using Naitrust.Domain.Models.Dtos.Requests.Payments;
using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<NaitrustResponse<VirtualAccountResponse>> CreateVirtualAccountAsync(CreateVirtualAccountRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PaymentStatusResponse>> GetPaymentStatusAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<ReleaseRequestResponse>> RequestReleaseAsync(Guid transactionId, RequestReleaseRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<LedgerEntryResponse>>> GetLedgerAsync(Guid transactionId, PaginationRequest pagination, CancellationToken ct = default);
    Task<NaitrustResponse<ReconciliationStatusResponse>> GetReconciliationStatusAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<PayoutAccountValidationResponse>> ValidatePayoutAccountAsync(ValidatePayoutAccountRequest request, CancellationToken ct = default);
}
