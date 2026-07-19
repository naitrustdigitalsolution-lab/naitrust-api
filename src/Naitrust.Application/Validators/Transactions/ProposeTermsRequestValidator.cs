using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Application.Validators.Transactions;

public class ProposeTermsRequestValidator : AbstractValidator<ProposeTermsRequest>
{
    public ProposeTermsRequestValidator()
    {
        RuleFor(x => x.Summary).MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(5000);
        RuleFor(x => x.DeliveryConditions).MaximumLength(2000);
        RuleFor(x => x.ReleaseConditions).MaximumLength(2000);
        RuleFor(x => x.ProofRequirements).MaximumLength(2000);
        RuleFor(x => x.DisputeRules).MaximumLength(2000);
        RuleFor(x => x.AutoConfirmWindowHours).GreaterThan(0).LessThanOrEqualTo(720).When(x => x.AutoConfirmWindowHours.HasValue);
    }
}
