using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Dtos.Responses.Disputes;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Public;
using Naitrust.Domain.Models.Dtos.Responses.Verification;

namespace Naitrust.Application.Services.Interfaces;

public interface IAdminService
{
    /// <summary>
    /// Retrieves a paginated list of all deals for administrative review.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<DealResponse>>> GetDealsAsync(PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single deal by ID with full details for administrative review.
    /// </summary>
    Task<NaitrustResponse<DealResponse>> GetDealAsync(Guid dealId, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a paginated list of all disputes for administrative review.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<DisputeResponse>>> GetDisputesAsync(PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Resolves a dispute with an administrative decision (approve, reject, escalate).
    /// </summary>
    Task<NaitrustResponse<DisputeResponse>> ResolveDisputeAsync(Guid disputeId, ResolveAdminDisputeRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a paginated list of all verification requests for administrative review.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<VerificationRequestResponse>>> GetVerificationsAsync(PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Updates a verification request's status (approve, reject, request more info).
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> UpdateVerificationAsync(Guid verificationId, UpdateAdminVerificationRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves paginated system audit logs for administrative review.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a paginated list of all waitlist entries.
    /// </summary>
    Task<NaitrustResponse<PaginatedResponse<WaitlistEntryResponse>>> GetWaitlistAsync(PaginationRequest pagination, CancellationToken ct = default);

    /// <summary>
    /// One-time setup: provisions the platform's central escrow subledger on Anchor.
    /// Idempotent — returns existing record if already set up.
    /// </summary>
    Task<NaitrustResponse<EscrowSetupResponse>> SetupPlatformEscrowAsync(CancellationToken ct = default);
}
