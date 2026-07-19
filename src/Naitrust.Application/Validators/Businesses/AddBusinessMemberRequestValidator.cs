using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Businesses;

namespace Naitrust.Application.Validators.Businesses;

public class AddBusinessMemberRequestValidator : AbstractValidator<AddBusinessMemberRequest>
{
    public AddBusinessMemberRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Role).NotEmpty().MaximumLength(50);
    }
}
