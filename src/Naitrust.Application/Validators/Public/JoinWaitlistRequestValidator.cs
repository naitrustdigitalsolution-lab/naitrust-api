using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Public;

namespace Naitrust.Application.Validators.Public;

public class JoinWaitlistRequestValidator : AbstractValidator<JoinWaitlistRequest>
{
    public JoinWaitlistRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
        RuleFor(x => x.Source).MaximumLength(100).When(x => x.Source is not null);
        RuleFor(x => x.BusinessName).MaximumLength(300).When(x => x.BusinessName is not null);
        RuleFor(x => x.UserType).MaximumLength(100).When(x => x.UserType is not null);
        RuleFor(x => x.TransactionRange).MaximumLength(50).When(x => x.TransactionRange is not null);
        RuleFor(x => x.TransactionNeed).MaximumLength(2000).When(x => x.TransactionNeed is not null);
        RuleFor(x => x.Expectations).MaximumLength(2000).When(x => x.Expectations is not null);
    }
}
