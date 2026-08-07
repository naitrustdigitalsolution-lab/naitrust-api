using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;

namespace Naitrust.Application.Validators.Disputes;

public class OpenDisputeRequestValidator : AbstractValidator<OpenDisputeRequest>
{
    public OpenDisputeRequestValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(5000);
    }
}
