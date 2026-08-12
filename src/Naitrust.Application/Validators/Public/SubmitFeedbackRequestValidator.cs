using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Public;

namespace Naitrust.Application.Validators.Public;

public class SubmitFeedbackRequestValidator : AbstractValidator<SubmitFeedbackRequest>
{
    public SubmitFeedbackRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Message).MaximumLength(5000).When(x => x.Message is not null);
        RuleFor(x => x.Name).MaximumLength(200).When(x => x.Name is not null);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(320).When(x => x.Email is not null);
    }
}
