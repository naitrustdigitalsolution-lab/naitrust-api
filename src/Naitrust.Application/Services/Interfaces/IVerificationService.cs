using Naitrust.Domain.Models.Dtos.Requests.Verification;
using Naitrust.Domain.Models.Dtos.Responses.Verification;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Application.Services.Interfaces;

public interface IVerificationService
{
    /// <summary>
    /// Starts a new KYC/AML verification request for the authenticated user.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> StartVerificationAsync(Guid userId, StartVerificationRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the overall verification status and completed steps for a verification request.
    /// </summary>
    Task<NaitrustResponse<VerificationStatusResponse>> GetVerificationStatusAsync(Guid verificationId, CancellationToken ct = default);

    /// <summary>
    /// Submits individual identity verification data (name, date of birth, ID number).
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitIndividualAsync(Guid verificationId, Guid userId, IndividualVerificationRequest request, CancellationToken ct = default);

    /// <summary>
    /// Submits business verification data (company registration, tax ID, directors).
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitBusinessAsync(Guid verificationId, Guid userId, BusinessVerificationRequest request, CancellationToken ct = default);

    /// <summary>
    /// Submits facial verification data (selfie image) for liveness and face-match checks.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitFacialAsync(Guid verificationId, Guid userId, FacialVerificationRequest request, CancellationToken ct = default);

    /// <summary>
    /// Uploads supporting documents (government ID, utility bill, etc.) for a verification step.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> UploadDocumentsAsync(Guid verificationId, Guid userId, UploadVerificationDocumentRequest request, Stream fileStream, string fileName, CancellationToken ct = default);

    /// <summary>
    /// Submits ownership verification data linking a user to a business entity.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> SubmitOwnershipAsync(Guid verificationId, Guid userId, OwnershipVerificationRequest request, CancellationToken ct = default);

    /// <summary>
    /// Verifies a one-time code sent via SMS or email during the verification process.
    /// </summary>
    Task<NaitrustResponse<bool>> VerifyCodeAsync(Guid verificationId, Guid userId, VerifyCodeRequest request, CancellationToken ct = default);

    /// <summary>
    /// Retrieves full details of a verification request including all steps and their statuses.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> GetRequestAsync(Guid verificationId, CancellationToken ct = default);

    /// <summary>
    /// Triggers the automated verification process by calling external verification providers.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> RunVerificationAsync(Guid verificationId, CancellationToken ct = default);

    /// <summary>
    /// Requests additional information from the user for a pending verification step.
    /// </summary>
    Task<NaitrustResponse<VerificationRequestResponse>> RequestMoreInfoAsync(Guid verificationId, RequestMoreInfoRequest request, CancellationToken ct = default);

    /// <summary>
    /// Checks whether a user has a valid, reusable verification of the specified type.
    /// </summary>
    Task<NaitrustResponse<bool>> CheckReusableVerificationAsync(Guid userId, string verificationType, CancellationToken ct = default);

    /// <summary>
    /// Checks whether the user's most recent liveness verification is still considered fresh.
    /// </summary>
    Task<NaitrustResponse<bool>> CheckLivenessFreshnessAsync(Guid userId, CancellationToken ct = default);
}
