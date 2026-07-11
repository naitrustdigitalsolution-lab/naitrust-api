using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface ITransactionService
{
    Task<NaitrustResponse<TransactionResponse>> CreateTransactionAsync(Guid userId, CreateTransactionRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> GetTransactionAsync(Guid transactionId, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<TransactionResponse>>> ListTransactionsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default);
    Task<NaitrustResponse<TransactionResponse>> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<List<TransactionTypeResponse>>> GetTransactionTypesAsync(CancellationToken ct = default);
}
