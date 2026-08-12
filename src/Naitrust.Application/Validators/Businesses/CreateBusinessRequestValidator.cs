using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;

namespace Naitrust.Application.Validators.Businesses;

public class CreateBusinessRequestValidator : AbstractValidator<CreateBusinessRequest>
{
    public CreateBusinessRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).MaximumLength(250);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.OwnerName).MaximumLength(200);
        RuleFor(x => x.Email).MaximumLength(320).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.Website).MaximumLength(500);
        RuleFor(x => x.RegistrationNumber).MaximumLength(50);
        RuleFor(x => x.TaxId).MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).MaximumLength(100);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.SocialHandles).MaximumLength(2000);
        RuleFor(x => x.PaymentAccountBankName).MaximumLength(200);
        RuleFor(x => x.PaymentAccountNumber).MaximumLength(50);
        RuleFor(x => x.PaymentAccountName).MaximumLength(200);
    }
}
