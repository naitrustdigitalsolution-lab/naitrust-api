namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record ProposeTermsRequest(
    string? Summary,
    string? Description,
    string? DeliveryConditions,
    string? ReleaseConditions,
    string? ProofRequirements,
    string? DisputeRules,
    DateTime? DeliveryDueAt,
    int? AutoConfirmWindowHours);
