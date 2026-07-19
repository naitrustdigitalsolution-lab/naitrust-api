using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Application.Validators.Transactions;

public class InvitePartyRequestValidator : AbstractValidator<InvitePartyRequest>
{
    public InvitePartyRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().MaximumLength(320).When(x => x.Email is not null);
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
        RuleFor(x => x.PartyType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DisplayName).MaximumLength(200).When(x => x.DisplayName is not null);
    }
}
