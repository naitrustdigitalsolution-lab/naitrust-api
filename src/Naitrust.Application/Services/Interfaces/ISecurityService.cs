using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Security;
using Naitrust.Domain.Models.Dtos.Responses.Security;

namespace Naitrust.Application.Services.Interfaces;

public interface ISecurityService
{
    /// <summary>Sends an OTP to the specified email for verification.</summary>
    Task<NaitrustResponse<bool>> SendEmailOtpAsync(Guid userId, SendEmailOtpRequest request, CancellationToken ct = default);

    /// <summary>Validates the email OTP and marks the user's email as verified.</summary>
    Task<NaitrustResponse<bool>> VerifyEmailOtpAsync(Guid userId, VerifyEmailOtpRequest request, CancellationToken ct = default);

    /// <summary>Sends an OTP via SMS to the specified phone number.</summary>
    Task<NaitrustResponse<bool>> SendPhoneOtpAsync(Guid userId, SendPhoneOtpRequest request, CancellationToken ct = default);

    /// <summary>Validates the phone OTP and marks the user's phone as verified.</summary>
    Task<NaitrustResponse<bool>> VerifyPhoneOtpAsync(Guid userId, VerifyPhoneOtpRequest request, CancellationToken ct = default);

    /// <summary>Generates a TOTP secret and returns it with an otpauth URI for QR code display.</summary>
    Task<NaitrustResponse<Start2faSetupResponse>> Start2faAsync(Guid userId, Start2faRequest request, CancellationToken ct = default);

    /// <summary>Verifies the TOTP code and enables 2FA on the account if valid.</summary>
    Task<NaitrustResponse<bool>> Verify2faSetupAsync(Guid userId, Verify2faSetupRequest request, CancellationToken ct = default);

    /// <summary>Accepts a KYC submission and queues it for verification processing.</summary>
    Task<NaitrustResponse<bool>> SubmitKycAsync(Guid userId, SubmitKycRequest request, CancellationToken ct = default);

    /// <summary>Sets (or resets) the user's transaction PIN.</summary>
    Task<NaitrustResponse<bool>> SetPinAsync(Guid userId, SetPinRequest request, CancellationToken ct = default);

    /// <summary>Verifies the user's transaction PIN without side effects.</summary>
    Task<NaitrustResponse<bool>> VerifyPinAsync(Guid userId, VerifyPinRequest request, CancellationToken ct = default);

    /// <summary>Registers a deal-scoped liveness capture (e.g. at deal creation), optionally with a photo uploaded to storage.</summary>
    Task<NaitrustResponse<DealIdentityCaptureResponse>> RegisterDealIdentityCaptureAsync(
        Guid userId, RegisterDealIdentityCaptureRequest request, Stream? photoStream, string? photoFileName, string? photoContentType, CancellationToken ct = default);
}
