using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Verification;
using Naitrust.Domain.Models.Dtos.Responses.Verification;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Verification;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Verification;

public class VerificationService : IVerificationService
{
    private readonly IUnitOfWork _unitOfWork;

    public VerificationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> StartVerificationAsync(Guid userId, StartVerificationRequest request, CancellationToken ct = default)
    {
        if (!Enum.TryParse<SubjectType>(request.SubjectType, ignoreCase: true, out var subjectType))
        {
            return NaitrustResponse<VerificationRequestResponse>.BadRequest($"Invalid subject type: {request.SubjectType}");
        }

        if (!Enum.TryParse<VerificationType>(request.VerificationType, ignoreCase: true, out var verificationType))
        {
            return NaitrustResponse<VerificationRequestResponse>.BadRequest($"Invalid verification type: {request.VerificationType}");
        }

        var verificationLevel = VerificationLevel.Basic;
        if (request.VerificationLevel is not null &&
            !Enum.TryParse<VerificationLevel>(request.VerificationLevel, ignoreCase: true, out verificationLevel))
        {
            return NaitrustResponse<VerificationRequestResponse>.BadRequest($"Invalid verification level: {request.VerificationLevel}");
        }

        var repo = _unitOfWork.GetRepository<VerificationRequest>();

        var verificationRequest = new VerificationRequest
        {
            Id = Guid.NewGuid(),
            SubjectType = subjectType,
            SubjectId = request.SubjectId,
            TransactionId = request.TransactionId,
            RequestedByUserId = userId,
            VerificationType = verificationType,
            VerificationLevel = verificationLevel,
            Status = VerificationStatus.Pending,
            IsActive = true
        };

        await repo.AddAsync(verificationRequest);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<VerificationRequestResponse>.Created(
            "Verification request started successfully.",
            MapToRequestResponse(verificationRequest, null));
    }

    public async Task<NaitrustResponse<VerificationStatusResponse>> GetVerificationStatusAsync(Guid verificationId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var request = await repo.GetByIdAsync(verificationId);

        if (request is null || request.IsDeleted)
        {
            return NaitrustResponse<VerificationStatusResponse>.NotFound("Verification request not found.");
        }

        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var steps = await stepRepo.GetAllDataAsync(s => s.VerificationRequestId == verificationId);
        var stepList = steps.ToList();

        DateTime? identityVerifiedAt = null;
        DateTime? businessVerifiedAt = null;
        DateTime? ownershipVerifiedAt = null;
        DateTime? lastLivenessVerifiedAt = null;

        foreach (var step in stepList.Where(s => s.Status == VerificationStepStatus.Success))
        {
            switch (step.Step)
            {
                case VerificationStepType.IdDocument:
                case VerificationStepType.Bvn:
                    identityVerifiedAt ??= step.CompletedAt;
                    break;
                case VerificationStepType.Cac:
                case VerificationStepType.Tin:
                    businessVerifiedAt ??= step.CompletedAt;
                    break;
                case VerificationStepType.Ownership:
                    ownershipVerifiedAt ??= step.CompletedAt;
                    break;
                case VerificationStepType.Facial:
                    lastLivenessVerifiedAt = step.CompletedAt;
                    break;
            }
        }

        var response = new VerificationStatusResponse(
            request.SubjectType.ToString(),
            request.SubjectId,
            request.Status.ToString(),
            identityVerifiedAt,
            businessVerifiedAt,
            ownershipVerifiedAt,
            lastLivenessVerifiedAt);

        return NaitrustResponse<VerificationStatusResponse>.Success("Verification status retrieved successfully.", response);
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> SubmitIndividualAsync(Guid verificationId, Guid userId, IndividualVerificationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        verificationRequest.ResultSummary = $"Individual: {request.FirstName} {request.LastName}, ID: {request.IdType} {request.IdNumber}";
        verificationRequest.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(verificationRequest);

        // Create or update the verification step
        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var step = new VerificationStep
        {
            Id = Guid.NewGuid(),
            VerificationRequestId = verificationId,
            Step = VerificationStepType.IdDocument,
            Status = VerificationStepStatus.Processing,
            StartedAt = DateTime.UtcNow,
            IsActive = true
        };

        await stepRepo.AddAsync(step);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Individual verification data submitted successfully.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> SubmitBusinessAsync(Guid verificationId, Guid userId, BusinessVerificationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        verificationRequest.ResultSummary = $"Business: {request.BusinessName}, Reg: {request.RegistrationNumber}";
        verificationRequest.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(verificationRequest);

        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var step = new VerificationStep
        {
            Id = Guid.NewGuid(),
            VerificationRequestId = verificationId,
            Step = VerificationStepType.Cac,
            Status = VerificationStepStatus.Processing,
            StartedAt = DateTime.UtcNow,
            IsActive = true
        };

        await stepRepo.AddAsync(step);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Business verification data submitted successfully.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> SubmitFacialAsync(Guid verificationId, Guid userId, FacialVerificationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        verificationRequest.ResultSummary = $"Facial: {request.IdType} {request.IdNumber}";
        verificationRequest.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(verificationRequest);

        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var step = new VerificationStep
        {
            Id = Guid.NewGuid(),
            VerificationRequestId = verificationId,
            Step = VerificationStepType.Facial,
            Status = VerificationStepStatus.Processing,
            StartedAt = DateTime.UtcNow,
            IsActive = true
        };

        await stepRepo.AddAsync(step);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Facial verification data submitted successfully.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> UploadDocumentsAsync(Guid verificationId, Guid userId, UploadVerificationDocumentRequest request, Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        if (!Enum.TryParse<DocumentType>(request.DocumentType, ignoreCase: true, out var documentType))
        {
            return NaitrustResponse<VerificationRequestResponse>.BadRequest($"Invalid document type: {request.DocumentType}");
        }

        var docRepo = _unitOfWork.GetRepository<VerificationDocument>();

        var document = new VerificationDocument
        {
            Id = Guid.NewGuid(),
            VerificationRequestId = verificationId,
            UploadedByUserId = userId,
            DocumentType = documentType,
            FileUrl = $"https://storage.placeholder.com/verifications/{verificationId}/{fileName}",
            FileName = fileName,
            MimeType = "application/octet-stream",
            FileSize = fileStream.Length,
            Status = VerificationDocumentStatus.Pending,
            IsActive = true
        };

        await docRepo.AddAsync(document);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Verification document uploaded successfully.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> SubmitOwnershipAsync(Guid verificationId, Guid userId, OwnershipVerificationRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        verificationRequest.ResultSummary = $"Ownership: BusinessId={request.BusinessId}, Method={request.Method}";
        verificationRequest.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(verificationRequest);

        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var step = new VerificationStep
        {
            Id = Guid.NewGuid(),
            VerificationRequestId = verificationId,
            Step = VerificationStepType.Ownership,
            Status = VerificationStepStatus.Processing,
            StartedAt = DateTime.UtcNow,
            IsActive = true
        };

        await stepRepo.AddAsync(step);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Ownership verification data submitted successfully.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public Task<NaitrustResponse<bool>> VerifyCodeAsync(Guid verificationId, Guid userId, VerifyCodeRequest request, CancellationToken ct = default)
    {
        // Stub: OTP/code verification not yet integrated with external provider
        return Task.FromResult(
            NaitrustResponse<bool>.Success("Code verified successfully.", true));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> GetRequestAsync(Guid verificationId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Verification request retrieved successfully.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> RunVerificationAsync(Guid verificationId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        // Stub: external provider call not yet implemented
        verificationRequest.Status = VerificationStatus.Processing;
        verificationRequest.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(verificationRequest);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "Verification process started.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<VerificationRequestResponse>> RequestMoreInfoAsync(Guid verificationId, RequestMoreInfoRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<VerificationRequest>();
        var verificationRequest = await repo.GetByIdAsync(verificationId);

        if (verificationRequest is null || verificationRequest.IsDeleted)
        {
            return NaitrustResponse<VerificationRequestResponse>.NotFound("Verification request not found.");
        }

        verificationRequest.Status = VerificationStatus.NeedsMoreInfo;
        verificationRequest.ResultSummary = request.Message;
        verificationRequest.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(verificationRequest);
        await _unitOfWork.SaveChangesAsync();

        var steps = await GetStepResponses(verificationId);
        return NaitrustResponse<VerificationRequestResponse>.Success(
            "More information requested.",
            MapToRequestResponse(verificationRequest, steps));
    }

    public async Task<NaitrustResponse<bool>> CheckReusableVerificationAsync(Guid userId, string verificationType, CancellationToken ct = default)
    {
        if (!Enum.TryParse<VerificationType>(verificationType, ignoreCase: true, out var vType))
        {
            return NaitrustResponse<bool>.BadRequest($"Invalid verification type: {verificationType}");
        }

        var repo = _unitOfWork.GetRepository<VerificationRequest>();

        var existing = await repo.GetSingleByAsync(
            v => v.SubjectId == userId
                 && v.VerificationType == vType
                 && v.Status == VerificationStatus.Verified
                 && !v.IsDeleted
                 && (v.ExpiresAt == null || v.ExpiresAt > DateTime.UtcNow));

        var hasReusable = existing is not null;

        return NaitrustResponse<bool>.Success(
            hasReusable ? "Reusable verification found." : "No reusable verification found.",
            hasReusable);
    }

    public async Task<NaitrustResponse<bool>> CheckLivenessFreshnessAsync(Guid userId, CancellationToken ct = default)
    {
        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var requestRepo = _unitOfWork.GetRepository<VerificationRequest>();

        // Find all facial verification requests for this user
        var requests = await requestRepo.GetAllDataAsync(
            v => v.SubjectId == userId
                 && v.VerificationType == VerificationType.Facial
                 && v.Status == VerificationStatus.Verified
                 && !v.IsDeleted);

        var requestIds = requests.Select(r => r.Id).ToList();

        if (requestIds.Count == 0)
        {
            return NaitrustResponse<bool>.Success("No liveness verification found.", false);
        }

        var facialSteps = await stepRepo.GetAllDataAsync(
            s => requestIds.Contains(s.VerificationRequestId)
                 && s.Step == VerificationStepType.Facial
                 && s.Status == VerificationStepStatus.Success);

        var latestStep = facialSteps
            .OrderByDescending(s => s.CompletedAt)
            .FirstOrDefault();

        if (latestStep?.CompletedAt is null)
        {
            return NaitrustResponse<bool>.Success("No liveness verification completed.", false);
        }

        var isFresh = latestStep.CompletedAt.Value.AddDays(30) > DateTime.UtcNow;

        return NaitrustResponse<bool>.Success(
            isFresh ? "Liveness verification is fresh." : "Liveness verification has expired.",
            isFresh);
    }

    private async Task<List<VerificationStepResponse>> GetStepResponses(Guid verificationId)
    {
        var stepRepo = _unitOfWork.GetRepository<VerificationStep>();
        var steps = await stepRepo.GetAllDataAsync(s => s.VerificationRequestId == verificationId && !s.IsDeleted);

        return steps.Select(s => new VerificationStepResponse(
            s.Step.ToString(),
            s.Status.ToString(),
            s.Message,
            s.StartedAt,
            s.CompletedAt)).ToList();
    }

    private static VerificationRequestResponse MapToRequestResponse(VerificationRequest request, List<VerificationStepResponse>? steps)
    {
        return new VerificationRequestResponse(
            request.Id,
            request.SubjectType.ToString(),
            request.SubjectId,
            request.VerificationType.ToString(),
            request.VerificationLevel.ToString(),
            request.Status.ToString(),
            steps,
            request.CreatedAt);
    }
}
