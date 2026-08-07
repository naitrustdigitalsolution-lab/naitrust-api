using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(128);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Role).MaximumLength(50);
        RuleFor(x => x.BusinessName).MaximumLength(200);
    }
}
