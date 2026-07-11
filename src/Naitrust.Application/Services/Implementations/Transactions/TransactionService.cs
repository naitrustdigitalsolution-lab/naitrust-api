using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Application.Services.Implementations.Transactions;

// Read-only operations and types
public class TransactionService : ITransactionService
{
    public Task<NaitrustResponse<TransactionResponse>> CreateTransactionAsync(Guid userId, CreateTransactionRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> GetTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<TransactionResponse>>> ListTransactionsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<TransactionResponse>> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<List<TransactionTypeResponse>>> GetTransactionTypesAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
