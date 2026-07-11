using Naitrust.Domain.Models.Dtos.Requests.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Verification;
using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAdminService
{
    Task<NaitrustResponse<PaginatedResponse<TransactionResponse>>> GetTransactionsAsync(PaginationRequest pagination, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> GetTransactionAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<DisputeResponse>>> GetDisputesAsync(PaginationRequest pagination, CancellationToken ct = default);
    Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, ResolveAdminDisputeRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<VerificationRequestResponse>>> GetVerificationsAsync(PaginationRequest pagination, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> UpdateVerificationAsync(Guid verificationId, UpdateAdminVerificationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, CancellationToken ct = default);
}
