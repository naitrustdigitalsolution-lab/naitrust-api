using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Application.Validators.Payments;

public class RequestReleaseValidator : AbstractValidator<RequestReleaseRequest>
{
    public RequestReleaseValidator()
    {
        // TODO: Add validation rules
    }
}
