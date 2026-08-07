using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Application.Validators.Auth;

public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpRequest>
{
    public VerifyOtpRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Otp).NotEmpty().Length(6);
    }
}
