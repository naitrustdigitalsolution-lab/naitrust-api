using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Application.Validators.Transactions;

public class CreateDealRequestValidator : AbstractValidator<CreateDealRequest>
{
    public CreateDealRequestValidator()
    {
        RuleFor(x => x.UseCase).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DealType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Role).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.AmountMinor).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.DeliveryDueDate).MaximumLength(50);
        RuleFor(x => x.ReleaseConditions).MaximumLength(2000);
        RuleFor(x => x.ExpiresInDays).GreaterThan(0).When(x => x.ExpiresInDays.HasValue);

        RuleForEach(x => x.Participants).ChildRules(p =>
        {
            p.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            p.RuleFor(x => x.Email).MaximumLength(320).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        }).When(x => x.Participants is not null);
    }
}
