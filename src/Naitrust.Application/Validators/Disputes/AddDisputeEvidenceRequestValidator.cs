using FluentValidation;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;

namespace Naitrust.Application.Validators.Disputes;

public class AddDisputeEvidenceRequestValidator : AbstractValidator<AddDisputeEvidenceRequest>
{
    public AddDisputeEvidenceRequestValidator()
    {
        RuleFor(x => x.EvidenceFileId).NotEmpty();
    }
}
