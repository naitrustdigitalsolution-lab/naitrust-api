using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Application.Validators.Verification;

public class OwnershipVerificationRequestValidator : AbstractValidator<OwnershipVerificationRequest>
{
    public OwnershipVerificationRequestValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.Method).NotEmpty().MaximumLength(50);
    }
}
