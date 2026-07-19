using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Evidence;

namespace Naitrust.Application.Validators.Evidence;

public class UploadEvidenceRequestValidator : AbstractValidator<UploadEvidenceRequest>
{
    public UploadEvidenceRequestValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}
