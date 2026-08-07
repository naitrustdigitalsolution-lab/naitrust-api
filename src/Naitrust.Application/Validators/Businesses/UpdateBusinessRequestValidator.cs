using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;

namespace Naitrust.Application.Validators.Businesses;

public class UpdateBusinessRequestValidator : AbstractValidator<UpdateBusinessRequest>
{
    public UpdateBusinessRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200).When(x => x.Name is not null);
        RuleFor(x => x.Slug).MaximumLength(250).When(x => x.Slug is not null);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.OwnerName).MaximumLength(200).When(x => x.OwnerName is not null);
        RuleFor(x => x.Email).MaximumLength(320).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
        RuleFor(x => x.Website).MaximumLength(500).When(x => x.Website is not null);
        RuleFor(x => x.RegistrationNumber).MaximumLength(50).When(x => x.RegistrationNumber is not null);
        RuleFor(x => x.TaxId).MaximumLength(50).When(x => x.TaxId is not null);
        RuleFor(x => x.Country).MaximumLength(100).When(x => x.Country is not null);
        RuleFor(x => x.State).MaximumLength(100).When(x => x.State is not null);
        RuleFor(x => x.City).MaximumLength(100).When(x => x.City is not null);
        RuleFor(x => x.Address).MaximumLength(500).When(x => x.Address is not null);
        RuleFor(x => x.PaymentAccountBankName).MaximumLength(200).When(x => x.PaymentAccountBankName is not null);
        RuleFor(x => x.PaymentAccountNumber).MaximumLength(50).When(x => x.PaymentAccountNumber is not null);
        RuleFor(x => x.PaymentAccountName).MaximumLength(200).When(x => x.PaymentAccountName is not null);
    }
}
