using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Admin;

namespace Naitrust.Application.Services.Implementations.Ai;

public class AuditLogService : IAuditLogService
{
    public Task<NaitrustResponse<bool>> LogActionAsync(Guid? actorUserId, string action, string entityType, Guid entityId, string? before = null, string? after = null, string? ipAddress = null, string? userAgent = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, Guid? actorUserId = null, string? entityType = null, Guid? entityId = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
