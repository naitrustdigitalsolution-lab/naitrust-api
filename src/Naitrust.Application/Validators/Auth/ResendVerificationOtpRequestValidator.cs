using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Application.Validators.Auth;

public class ResendVerificationOtpRequestValidator : AbstractValidator<ResendVerificationOtpRequest>
{
    public ResendVerificationOtpRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
