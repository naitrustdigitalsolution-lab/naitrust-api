using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Application.Validators.Payments;

public class CreateSettlementAccountRequestValidator : AbstractValidator<CreateSettlementAccountRequest>
{
    public CreateSettlementAccountRequestValidator()
    {
        RuleFor(x => x.PartnerId).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BusinessId).Must(id => id != Guid.Empty).When(x => x.BusinessId.HasValue)
            .WithMessage("BusinessId must not be empty when provided.");
    }
}
