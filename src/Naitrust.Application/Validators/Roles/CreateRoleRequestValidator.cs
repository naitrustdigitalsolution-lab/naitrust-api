using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Roles;

namespace Naitrust.Application.Validators.Roles;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
