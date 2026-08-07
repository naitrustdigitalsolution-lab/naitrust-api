namespace Naitrust.Domain.Models.Dtos.Requests.Negotiations;

public record ProposedChangesInput(
    long? AmountMinor,
    string? DeliveryDueDate,
    string? ReleaseConditions,
    string? AgreementNote);
