using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Application.Validators.Auth;

public class Verify2FARequestValidator : AbstractValidator<Verify2FARequest>
{
    public Verify2FARequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.UserId.HasValue || !string.IsNullOrEmpty(x.Email))
            .WithMessage("Either UserId or Email must be provided.");
    }
}
