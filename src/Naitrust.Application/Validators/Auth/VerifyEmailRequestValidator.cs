using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Application.Validators.Auth;

public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Otp).NotEmpty().Length(6);
    }
}
