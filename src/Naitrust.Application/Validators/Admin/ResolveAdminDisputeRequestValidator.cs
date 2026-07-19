using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Admin;

namespace Naitrust.Application.Validators.Admin;

public class ResolveAdminDisputeRequestValidator : AbstractValidator<ResolveAdminDisputeRequest>
{
    public ResolveAdminDisputeRequestValidator()
    {
        RuleFor(x => x.Resolution).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Reason).MaximumLength(2000);
    }
}
