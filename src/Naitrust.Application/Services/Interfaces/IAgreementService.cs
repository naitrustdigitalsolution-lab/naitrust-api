using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Agreements;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Application.Services.Interfaces;

public interface IAgreementService
{
    Task<NaitrustResponse<AgreementResponse>> DraftAgreementAsync(DraftAgreementRequest request, CancellationToken ct = default);
}
