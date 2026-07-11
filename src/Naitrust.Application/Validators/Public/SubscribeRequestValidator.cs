using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Public;

namespace Naitrust.Application.Validators.Public;

public class SubscribeRequestValidator : AbstractValidator<SubscribeRequest>
{
    public SubscribeRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
    }
}
