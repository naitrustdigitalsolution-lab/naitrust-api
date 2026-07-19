using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;

namespace Naitrust.Application.Validators.Businesses;

public class CreateBusinessRequestValidator : AbstractValidator<CreateBusinessRequest>
{
    public CreateBusinessRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
        RuleFor(x => x.RegistrationNumber).MaximumLength(50);
        RuleFor(x => x.TaxId).MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).MaximumLength(100);
        RuleFor(x => x.Address).MaximumLength(500);
    }
}
