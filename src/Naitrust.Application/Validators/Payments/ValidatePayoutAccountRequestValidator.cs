using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Application.Validators.Payments;

public class ValidatePayoutAccountRequestValidator : AbstractValidator<ValidatePayoutAccountRequest>
{
    public ValidatePayoutAccountRequestValidator()
    {
        RuleFor(x => x.BankCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.AccountNumber).NotEmpty().MaximumLength(20);
    }
}
