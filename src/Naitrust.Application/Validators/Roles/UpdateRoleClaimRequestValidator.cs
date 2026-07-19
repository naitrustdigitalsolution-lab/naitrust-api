using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Roles;

namespace Naitrust.Application.Validators.Roles;

public class UpdateRoleClaimRequestValidator : AbstractValidator<UpdateRoleClaimRequest>
{
    public UpdateRoleClaimRequestValidator()
    {
        RuleFor(x => x.Role).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ClaimType).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NewClaimType).NotEmpty().MaximumLength(100);
    }
}
