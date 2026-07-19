using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Roles;

namespace Naitrust.Application.Validators.Roles;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).When(x => x.Name is not null);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
    }
}
