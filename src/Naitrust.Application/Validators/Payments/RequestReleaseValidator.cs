using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Application.Validators.Payments;

public class RequestReleaseValidator : AbstractValidator<RequestReleaseRequest>
{
    public RequestReleaseValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.Reason).MaximumLength(2000);
    }
}
