using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Application.Validators.Verification;

public class IndividualVerificationRequestValidator : AbstractValidator<IndividualVerificationRequest>
{
    public IndividualVerificationRequestValidator()
    {
        RuleFor(x => x.IdType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IdNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(20);
    }
}
