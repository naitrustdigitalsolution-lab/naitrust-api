using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Public;

namespace Naitrust.Application.Validators.Public;

public class ContactUsRequestValidator : AbstractValidator<ContactUsRequest>
{
    public ContactUsRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(5000);
    }
}
