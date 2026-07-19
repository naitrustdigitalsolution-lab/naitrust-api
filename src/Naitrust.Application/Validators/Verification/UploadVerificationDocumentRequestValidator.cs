using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Application.Validators.Verification;

public class UploadVerificationDocumentRequestValidator : AbstractValidator<UploadVerificationDocumentRequest>
{
    public UploadVerificationDocumentRequestValidator()
    {
        RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(50);
    }
}
