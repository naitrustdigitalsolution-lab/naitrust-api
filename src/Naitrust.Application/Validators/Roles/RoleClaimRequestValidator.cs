using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Roles;

namespace Naitrust.Application.Validators.Roles;

public class RoleClaimRequestValidator : AbstractValidator<RoleClaimRequest>
{
    public RoleClaimRequestValidator()
    {
        RuleFor(x => x.Role).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ClaimType).NotEmpty().MaximumLength(100);
    }
}
