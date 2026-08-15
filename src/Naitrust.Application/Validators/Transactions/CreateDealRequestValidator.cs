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

        RuleFor(x => x.InitialPaymentMode)
            .Must(mode => mode == "fixed" || mode == "percentage")
            .When(x => x.InitialPaymentMode is not null)
            .WithMessage("InitialPaymentMode must be 'fixed' or 'percentage'.");
        RuleFor(x => x.InitialPaymentMinor)
            .GreaterThan(0).LessThan(x => x.AmountMinor)
            .When(x => x.InitialPaymentMinor.HasValue)
            .WithMessage("InitialPaymentMinor must be greater than 0 and less than the total amount.");
        RuleFor(x => x.InitialPaymentPercentage)
            .InclusiveBetween(1, 100)
            .When(x => x.InitialPaymentPercentage.HasValue);
        RuleFor(x => x.RemainingPaymentMinor)
            .GreaterThan(0)
            .When(x => x.RemainingPaymentMinor.HasValue);
        RuleFor(x => x.NextPaymentReleaseConditions).MaximumLength(2000);

        RuleForEach(x => x.Participants).ChildRules(p =>
        {
            p.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            p.RuleFor(x => x.Email).MaximumLength(320).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
            p.RuleForEach(x => x.PaymentAllocations).ChildRules(a =>
            {
                a.RuleFor(x => x.Stage).InclusiveBetween(1, 2);
                a.RuleFor(x => x.AmountMinor).GreaterThan(0);
            }).When(x => x.PaymentAllocations is not null);
        }).When(x => x.Participants is not null);
    }
}
