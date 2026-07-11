using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Public;
using Naitrust.Domain.Models.Dtos.Responses.Public;

namespace Naitrust.Application.Services.Interfaces;

public interface IPublicFormService
{
    Task<NaitrustResponse<PublicSubmissionResponse>> JoinWaitlistAsync(JoinWaitlistRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PublicSubmissionResponse>> ContactUsAsync(ContactUsRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PublicSubmissionResponse>> SubscribeAsync(SubscribeRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PublicSubmissionResponse>> SubmitFeedbackAsync(SubmitFeedbackRequest request, CancellationToken ct = default);
    Task<NaitrustResponse<PublicSubmissionResponse>> ReportConcernAsync(ReportConcernRequest request, CancellationToken ct = default);
}
