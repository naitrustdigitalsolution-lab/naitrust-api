using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Admin;

namespace Naitrust.Application.Validators.Admin;

public class UpdateAdminVerificationRequestValidator : AbstractValidator<UpdateAdminVerificationRequest>
{
    public UpdateAdminVerificationRequestValidator()
    {
        RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ReviewNotes).MaximumLength(2000);
    }
}
