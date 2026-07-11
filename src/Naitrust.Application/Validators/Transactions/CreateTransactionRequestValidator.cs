using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;

namespace Naitrust.Application.Validators.Transactions;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        // TODO: Add validation rules
    }
}
