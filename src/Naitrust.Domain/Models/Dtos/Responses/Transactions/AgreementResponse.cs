namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record AgreementResponse(
    Guid Id,
    int Version,
    string? Summary,
    string? Description,
    string? DeliveryConditions,
    string? ReleaseConditions,
    string? ProofRequirements,
    string? DisputeRules,
    int? AutoConfirmWindowHours,
    DateTime? DeliveryDueAt,
    DateTime? FrozenAt,
    DateTime CreatedAt);
