using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface ITransactionService
{
    /// <summary>
    /// Creates a new transaction in Draft status and assigns the creator as the initiating party.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> CreateTransactionAsync(Guid userId, CreateTransactionRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a transaction by ID, including parties, agreement, milestones, and allowed actions.
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> GetTransactionAsync(Guid transactionId, CancellationToken ct = default);

    /// <summary>
    /// Lists all transactions where the authenticated user is a party, with pagination support.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<TransactionResponse>>> ListTransactionsAsync(Guid userId, PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Updates a transaction's title, description, or amount (only allowed while in Draft status).
    /// </summary>
    Task<NaitrustResponse<TransactionResponse>> UpdateTransactionAsync(Guid transactionId, UpdateTransactionRequest request, CancellationToken ct = default);

    /// <summary>
    /// Returns all available transaction types (e.g., Goods, Services, Real Estate).
    /// </summary>
    Task<NaitrustResponse<List<TransactionTypeResponse>>> GetTransactionTypesAsync(CancellationToken ct = default);
}
