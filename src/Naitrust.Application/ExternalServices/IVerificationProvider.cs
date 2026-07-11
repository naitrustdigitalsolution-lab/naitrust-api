namespace Naitrust.Application.ExternalServices;

public interface IVerificationProvider
{
    Task<object> VerifyIndividualAsync(object request, CancellationToken ct = default);
    Task<object> VerifyBusinessAsync(object request, CancellationToken ct = default);
    Task<object> VerifyFaceAsync(object request, CancellationToken ct = default);
    Task<object> CheckOwnershipAsync(object request, CancellationToken ct = default);
    Task<object> SendOtpAsync(object request, CancellationToken ct = default);
    Task<object> VerifyOtpAsync(object request, CancellationToken ct = default);
}
