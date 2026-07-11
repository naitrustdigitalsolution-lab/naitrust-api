using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Verification;

namespace Naitrust.Application.Services.Implementations.Admin;

public class AdminService : IAdminService
{
    public Task<NaitrustResponse<PaginatedResponse<TransactionResponse>>> GetTransactionsAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> GetTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<DisputeResponse>>> GetDisputesAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, ResolveAdminDisputeRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<VerificationRequestResponse>>> GetVerificationsAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> UpdateVerificationAsync(Guid verificationId, UpdateAdminVerificationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
