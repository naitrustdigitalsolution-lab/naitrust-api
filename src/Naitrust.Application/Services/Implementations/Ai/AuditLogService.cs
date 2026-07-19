using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Admin;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Ai;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditLogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<bool>> LogActionAsync(Guid? actorUserId, string action, string entityType, Guid entityId, string? before = null, string? after = null, string? ipAddress = null, string? userAgent = null, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<AuditLog>();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actorUserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Before = before,
            After = after,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(auditLog);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Audit log entry created successfully.", true);
    }

    public async Task<NaitrustResponse<PaginatedResponse<AuditLogResponse>>> GetAuditLogsAsync(PaginationRequest pagination, Guid? actorUserId = null, string? entityType = null, Guid? entityId = null, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<AuditLog>();

        var allLogs = await repo.GetAllDataAsync(
            predicate: a =>
                (!actorUserId.HasValue || a.ActorUserId == actorUserId.Value) &&
                (entityType == null || a.EntityType == entityType) &&
                (!entityId.HasValue || a.EntityId == entityId.Value),
            orderBy: q => q.OrderByDescending(a => a.CreatedAt));

        var logList = allLogs.ToList();
        var totalCount = logList.Count;

        var pagedLogs = logList
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var responses = pagedLogs.Select(a => new AuditLogResponse(
            a.Id,
            a.ActorUserId,
            a.Action,
            a.EntityType,
            a.EntityId,
            a.IpAddress,
            a.CreatedAt)).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return NaitrustResponse<PaginatedResponse<AuditLogResponse>>.Success(
            "Audit logs retrieved successfully.",
            new PaginatedResponse<AuditLogResponse>(responses, pagination.Page, pagination.PageSize, totalCount, totalPages));
    }
}
