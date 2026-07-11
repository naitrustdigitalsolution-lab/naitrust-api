using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IAuditLogService
{
    Task<NaitrustResponse<bool>> LogActionAsync(Guid? actorUserId, string action, string entityType, Guid entityId, string? before = null, string? after = null, string? ipAddress = null, string? userAgent = null, CancellationToken ct = default);
    Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, Guid? actorUserId = null, string? entityType = null, Guid? entityId = null, CancellationToken ct = default);
}
