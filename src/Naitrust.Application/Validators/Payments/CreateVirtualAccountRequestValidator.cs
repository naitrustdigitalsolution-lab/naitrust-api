using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Payments;

namespace Naitrust.Application.Validators.Payments;

public class CreateVirtualAccountRequestValidator : AbstractValidator<CreateVirtualAccountRequest>
{
    public CreateVirtualAccountRequestValidator()
    {
        // TODO: Add validation rules
    }
}
