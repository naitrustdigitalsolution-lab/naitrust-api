using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Verification;
using Naitrust.Domain.Models.Dtos.Responses.Verification;

namespace Naitrust.Application.Services.Implementations.Verification;

public class VerificationService : IVerificationService
{
    public Task<NaitrustResponse<VerificationRequestResponse>> StartVerificationAsync(Guid userId, StartVerificationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationStatusResponse>> GetVerificationStatusAsync(Guid verificationId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> SubmitIndividualAsync(Guid verificationId, Guid userId, IndividualVerificationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> SubmitBusinessAsync(Guid verificationId, Guid userId, BusinessVerificationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> SubmitFacialAsync(Guid verificationId, Guid userId, FacialVerificationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> UploadDocumentsAsync(Guid verificationId, Guid userId, UploadVerificationDocumentRequest request, Stream fileStream, string fileName, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> SubmitOwnershipAsync(Guid verificationId, Guid userId, OwnershipVerificationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> VerifyCodeAsync(Guid verificationId, Guid userId, VerifyCodeRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> GetRequestAsync(Guid verificationId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> RunVerificationAsync(Guid verificationId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<VerificationRequestResponse>> RequestMoreInfoAsync(Guid verificationId, RequestMoreInfoRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> CheckReusableVerificationAsync(Guid userId, string verificationType, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<NaitrustResponse<bool>> CheckLivenessFreshnessAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
