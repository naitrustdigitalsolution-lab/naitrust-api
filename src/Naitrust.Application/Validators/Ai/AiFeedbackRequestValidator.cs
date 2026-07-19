using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Ai;

namespace Naitrust.Application.Validators.Ai;

public class AiFeedbackRequestValidator : AbstractValidator<AiFeedbackRequest>
{
    public AiFeedbackRequestValidator()
    {
        RuleFor(x => x.AssessmentId).NotEmpty();
        RuleFor(x => x.FeedbackType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}
