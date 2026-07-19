using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;

namespace Naitrust.Application.Validators.Disputes;

public class AddDisputeMessageRequestValidator : AbstractValidator<AddDisputeMessageRequest>
{
    public AddDisputeMessageRequestValidator()
    {
        RuleFor(x => x.Message).NotEmpty().MaximumLength(5000);
    }
}
