using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Evidence;
using Naitrust.Domain.Models.Dtos.Responses.Evidence;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Evidence;

public class EvidenceService : IEvidenceService
{
    private readonly IUnitOfWork _unitOfWork;

    public EvidenceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<EvidenceFileResponse>> UploadEvidenceAsync(Guid userId, UploadEvidenceRequest request, Stream fileStream, string fileName, CancellationToken ct = default)
    {
        if (!Enum.TryParse<EvidenceType>(request.Type, ignoreCase: true, out var evidenceType))
        {
            return NaitrustResponse<EvidenceFileResponse>.BadRequest($"Invalid evidence type: {request.Type}");
        }

        var dealRepo = _unitOfWork.GetRepository<Deal>();
        var deal = await dealRepo.GetByIdAsync(request.TransactionId);

        if (deal is null || deal.IsDeleted)
        {
            return NaitrustResponse<EvidenceFileResponse>.NotFound("Deal not found.");
        }

        var repo = _unitOfWork.GetRepository<EvidenceFile>();

        // Stub: file storage not yet integrated — use placeholder URL
        var evidenceFile = new EvidenceFile
        {
            Id = Guid.NewGuid(),
            TransactionId = request.TransactionId,
            MilestoneId = request.MilestoneId,
            UploadedByUserId = userId,
            Type = evidenceType,
            FileUrl = $"https://storage.placeholder.com/evidence/{request.TransactionId}/{fileName}",
            FileName = fileName,
            MimeType = "application/octet-stream",
            FileSize = fileStream.Length,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(evidenceFile);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<EvidenceFileResponse>.Created(
            "Evidence uploaded successfully.",
            MapToResponse(evidenceFile));
    }

    public async Task<NaitrustResponse<EvidenceFileResponse>> GetEvidenceAsync(Guid evidenceId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<EvidenceFile>();
        var evidenceFile = await repo.GetByIdAsync(evidenceId);

        if (evidenceFile is null)
        {
            return NaitrustResponse<EvidenceFileResponse>.NotFound("Evidence file not found.");
        }

        return NaitrustResponse<EvidenceFileResponse>.Success(
            "Evidence file retrieved successfully.",
            MapToResponse(evidenceFile));
    }

    public async Task<NaitrustResponse<List<EvidenceFileResponse>>> ListEvidenceByTransactionAsync(Guid transactionId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<EvidenceFile>();
        var files = await repo.GetAllDataAsync(
            e => e.TransactionId == transactionId,
            orderBy: q => q.OrderByDescending(e => e.CreatedAt));

        var responses = files.Select(MapToResponse).ToList();

        return NaitrustResponse<List<EvidenceFileResponse>>.Success(
            "Evidence files retrieved successfully.", responses);
    }

    private static EvidenceFileResponse MapToResponse(EvidenceFile file)
    {
        return new EvidenceFileResponse(
            file.Id,
            file.TransactionId,
            file.MilestoneId,
            file.UploadedByUserId,
            file.Type.ToString(),
            file.FileUrl,
            file.FileName,
            file.Description,
            file.CreatedAt);
    }
}
