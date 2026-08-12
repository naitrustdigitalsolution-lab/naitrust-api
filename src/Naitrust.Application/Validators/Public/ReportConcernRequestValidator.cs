using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Public;

namespace Naitrust.Application.Validators.Public;

public class ReportConcernRequestValidator : AbstractValidator<ReportConcernRequest>
{
    public ReportConcernRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200).When(x => x.Name is not null);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Category).MaximumLength(100).When(x => x.Category is not null);
        RuleFor(x => x.Description).MaximumLength(5000).When(x => x.Description is not null);
    }
}
