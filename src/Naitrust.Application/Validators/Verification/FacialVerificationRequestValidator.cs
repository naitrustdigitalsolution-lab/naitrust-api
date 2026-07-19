using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Application.Validators.Verification;

public class FacialVerificationRequestValidator : AbstractValidator<FacialVerificationRequest>
{
    public FacialVerificationRequestValidator()
    {
        RuleFor(x => x.IdType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IdNumber).NotEmpty().MaximumLength(50);
    }
}
