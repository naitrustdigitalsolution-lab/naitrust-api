using Naitrust.Domain.Models.Dtos.Requests.Payments;
using Naitrust.Domain.Models.Dtos.Responses.Payments;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IPaymentService
{
    /// <summary>
    /// Creates a settlement virtual account for a user or business.
    /// </summary>
    Task<NaitrustResponse<VirtualAccountResponse>> CreateSettlementAccountAsync(Guid userId, CreateSettlementAccountRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the settlement account for a user or business.
    /// </summary>
    Task<NaitrustResponse<VirtualAccountResponse>> GetSettlementAccountAsync(Guid userId, Guid? businessId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the current payment status for a transaction (funded, released, etc.).
    /// </summary>
    Task<NaitrustResponse<PaymentStatusResponse>> GetPaymentStatusAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Submits a request to release escrowed funds to the designated party.
    /// </summary>
    Task<NaitrustResponse<ReleaseRequestResponse>> RequestReleaseAsync(Guid transactionId, RequestReleaseRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves paginated ledger entries (double-entry accounting) for a transaction.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<LedgerEntryResponse>>> GetLedgerAsync(Guid transactionId, PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Returns the reconciliation status between internal records and payment partner data.
    /// </summary>
    Task<NaitrustResponse<ReconciliationStatusResponse>> GetReconciliationStatusAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Validates a payout (bank) account with the payment partner before enabling withdrawals.
    /// </summary>
    Task<NaitrustResponse<PayoutAccountValidationResponse>> ValidatePayoutAccountAsync(ValidatePayoutAccountRequest request, CancellationToken ct = default);
}
