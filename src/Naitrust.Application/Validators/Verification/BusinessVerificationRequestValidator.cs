using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Application.Validators.Verification;

public class BusinessVerificationRequestValidator : AbstractValidator<BusinessVerificationRequest>
{
    public BusinessVerificationRequestValidator()
    {
        RuleFor(x => x.RegistrationType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.RegistrationNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BusinessName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Tin).MaximumLength(50);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.OwnerFirstName).MaximumLength(100);
        RuleFor(x => x.OwnerLastName).MaximumLength(100);
        RuleFor(x => x.OwnerIdType).MaximumLength(50);
        RuleFor(x => x.OwnerIdNumber).MaximumLength(50);
    }
}
