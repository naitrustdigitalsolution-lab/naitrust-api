using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Application.Validators.Verification;

public class StartVerificationRequestValidator : AbstractValidator<StartVerificationRequest>
{
    public StartVerificationRequestValidator()
    {
        RuleFor(x => x.SubjectType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.VerificationType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.VerificationLevel).MaximumLength(50);
    }
}
