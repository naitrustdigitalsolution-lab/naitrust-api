using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;

namespace Naitrust.Application.Validators.Businesses;

public class UpdateBusinessMemberRequestValidator : AbstractValidator<UpdateBusinessMemberRequest>
{
    public UpdateBusinessMemberRequestValidator()
    {
        RuleFor(x => x.Role).MaximumLength(50);
        RuleFor(x => x.Status).MaximumLength(50);
    }
}
