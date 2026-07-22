using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Public;
using Naitrust.Domain.Models.Dtos.Responses.Public;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Public;

public class PublicFormService : IPublicFormService
{
    private readonly IUnitOfWork _unitOfWork;

    public PublicFormService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<PublicSubmissionResponse>> JoinWaitlistAsync(JoinWaitlistRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<WaitlistEntry>();

        var existing = await repo.GetSingleByAsync(x => x.Email == request.Email);
        if (existing is not null)
            return NaitrustResponse<PublicSubmissionResponse>.Conflict("This email is already on the waitlist.");

        var entry = new WaitlistEntry
        {
            Name = request.Name,
            Email = request.Email.ToLowerInvariant(),
            Phone = request.Phone,
            Source = request.Source,
            BusinessName = request.BusinessName,
            UserType = request.UserType,
            TransactionRange = request.TransactionRange,
            TransactionNeed = request.TransactionNeed,
            Expectations = request.Expectations,
            Consent = request.Consent,
            SubmittedAt = request.SubmittedAt ?? DateTime.UtcNow,
            IsActive = true
        };

        await repo.AddAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PublicSubmissionResponse>.Created(
            "Successfully joined the waitlist.",
            new PublicSubmissionResponse(entry.Id, "Successfully joined the waitlist.", entry.CreatedAt));
    }

    public async Task<NaitrustResponse<PublicSubmissionResponse>> ContactUsAsync(ContactUsRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<ContactMessage>();

        var entry = new ContactMessage
        {
            Name = request.Name,
            Email = request.Email.ToLowerInvariant(),
            Subject = request.Subject,
            Message = request.Message,
            IsActive = true
        };

        await repo.AddAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PublicSubmissionResponse>.Created(
            "Message received. We'll get back to you soon.",
            new PublicSubmissionResponse(entry.Id, "Message received. We'll get back to you soon.", entry.CreatedAt));
    }

    public async Task<NaitrustResponse<PublicSubmissionResponse>> SubscribeAsync(SubscribeRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<NewsletterSubscriber>();

        var existing = await repo.GetSingleByAsync(x => x.Email == request.Email);
        if (existing is not null)
            return NaitrustResponse<PublicSubmissionResponse>.Conflict("This email is already subscribed.");

        var entry = new NewsletterSubscriber
        {
            Email = request.Email.ToLowerInvariant(),
            IsActive = true
        };

        await repo.AddAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PublicSubmissionResponse>.Created(
            "Successfully subscribed to newsletter.",
            new PublicSubmissionResponse(entry.Id, "Successfully subscribed to newsletter.", entry.CreatedAt));
    }

    public async Task<NaitrustResponse<PublicSubmissionResponse>> SubmitFeedbackAsync(SubmitFeedbackRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Feedback>();

        var entry = new Feedback
        {
            Name = request.Name,
            Email = request.Email?.ToLowerInvariant(),
            Rating = request.Rating,
            Message = request.Message,
            IsActive = true
        };

        await repo.AddAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PublicSubmissionResponse>.Created(
            "Thank you for your feedback.",
            new PublicSubmissionResponse(entry.Id, "Thank you for your feedback.", entry.CreatedAt));
    }

    public async Task<NaitrustResponse<PublicSubmissionResponse>> ReportConcernAsync(ReportConcernRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<ReportedConcern>();

        var entry = new ReportedConcern
        {
            Name = request.Name,
            Email = request.Email.ToLowerInvariant(),
            Category = request.Category,
            Description = request.Description,
            IsActive = true
        };

        await repo.AddAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PublicSubmissionResponse>.Created(
            "Your concern has been reported. We'll review it promptly.",
            new PublicSubmissionResponse(entry.Id, "Your concern has been reported. We'll review it promptly.", entry.CreatedAt));
    }
}
