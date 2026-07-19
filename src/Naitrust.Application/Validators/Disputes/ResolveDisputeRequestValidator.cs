using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;

namespace Naitrust.Application.Validators.Disputes;

public class ResolveDisputeRequestValidator : AbstractValidator<ResolveDisputeRequest>
{
    public ResolveDisputeRequestValidator()
    {
        RuleFor(x => x.Resolution).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Reason).MaximumLength(2000);
    }
}
