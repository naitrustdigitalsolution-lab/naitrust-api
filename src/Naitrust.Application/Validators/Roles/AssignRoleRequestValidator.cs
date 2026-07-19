using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Roles;

namespace Naitrust.Application.Validators.Roles;

public class AssignRoleRequestValidator : AbstractValidator<AssignRoleRequest>
{
    public AssignRoleRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Role).NotEmpty().MaximumLength(50);
    }
}
