namespace Naitrust.Application.ExternalServices.QoreId;

public class QoreIdVerificationProvider : IVerificationProvider
{
    public Task<object> VerifyIndividualAsync(object request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<object> VerifyBusinessAsync(object request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<object> VerifyFaceAsync(object request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<object> CheckOwnershipAsync(object request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<object> SendOtpAsync(object request, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<object> VerifyOtpAsync(object request, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
