using Naitrust.Application.ExternalServices.QoreId;

namespace Naitrust.Application.ExternalServices;

public interface IVerificationProvider
{
    Task<QoreIdBvnResult> VerifyBvnAsync(QoreIdBvnRequest request, CancellationToken ct = default);
    Task<QoreIdCacResult> VerifyCacAsync(QoreIdCacRequest request, CancellationToken ct = default);
}
