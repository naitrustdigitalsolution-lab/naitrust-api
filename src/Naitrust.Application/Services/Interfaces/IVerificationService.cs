using Naitrust.Domain.Models.Dtos.Requests.Verification;
using Naitrust.Domain.Models.Dtos.Responses.Verification;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IVerificationService
{
    Task<NaitrustResponse<VerificationRequestResponse>> StartVerificationAsync(Guid userId, StartVerificationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationStatusResponse>> GetVerificationStatusAsync(Guid verificationId, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitIndividualAsync(Guid verificationId, Guid userId, IndividualVerificationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitBusinessAsync(Guid verificationId, Guid userId, BusinessVerificationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitFacialAsync(Guid verificationId, Guid userId, FacialVerificationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> UploadDocumentsAsync(Guid verificationId, Guid userId, UploadVerificationDocumentRequest request, Stream fileStream, string fileName, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitOwnershipAsync(Guid verificationId, Guid userId, OwnershipVerificationRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> VerifyCodeAsync(Guid verificationId, Guid userId, VerifyCodeRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> GetRequestAsync(Guid verificationId, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> RunVerificationAsync(Guid verificationId, CancellationToken ct = default);
    Task<NaitrustResponse<VerificationRequestResponse>> RequestMoreInfoAsync(Guid verificationId, RequestMoreInfoRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> CheckReusableVerificationAsync(Guid userId, string verificationType, CancellationToken ct = default);
    Task<NaitrustResponse<bool>> CheckLivenessFreshnessAsync(Guid userId, CancellationToken ct = default);
}
