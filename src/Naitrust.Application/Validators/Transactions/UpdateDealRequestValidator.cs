using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Application.Validators.Transactions;

public class UpdateDealRequestValidator : AbstractValidator<UpdateDealRequest>
{
    public UpdateDealRequestValidator()
    {
        RuleFor(x => x.Title).MaximumLength(200).When(x => x.Title is not null);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.AmountMinor).GreaterThan(0).When(x => x.AmountMinor.HasValue);
    }
}
