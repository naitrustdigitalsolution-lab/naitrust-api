using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Application.Validators.Payments;

public class ValidatePayoutAccountRequestValidator : AbstractValidator<ValidatePayoutAccountRequest>
{
    public ValidatePayoutAccountRequestValidator()
    {
        // TODO: Add validation rules
    }
}
