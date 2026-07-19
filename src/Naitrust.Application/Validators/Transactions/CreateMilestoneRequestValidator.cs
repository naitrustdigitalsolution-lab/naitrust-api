using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Application.Validators.Transactions;

public class CreateMilestoneRequestValidator : AbstractValidator<CreateMilestoneRequest>
{
    public CreateMilestoneRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.AmountMinor).GreaterThan(0);
    }
}
