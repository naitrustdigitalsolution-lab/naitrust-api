using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.PaymentRequests;
using Naitrust.Domain.Models.Dtos.Responses.PaymentRequests;

namespace Naitrust.Application.Services.Interfaces;

public interface IPaymentRequestService
{
    Task<NaitrustResponse<List<PaymentRequestResponse>>> ListAsync(Guid userId, CancellationToken ct = default);
    Task<NaitrustResponse<PaymentRequestResponse>> CreateAsync(Guid userId, CreatePaymentRequestRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PaymentRequestResponse>> RespondAsync(Guid userId, Guid requestId, RespondPaymentRequestRequest request, CancellationToken ct = default);
}
