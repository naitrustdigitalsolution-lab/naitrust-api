namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record AgreementResponse(
    Guid Id,
    int Version,
    bool GeneratedByAi,
    List<AgreementSectionResponse> Sections);

public record AgreementSectionResponse(string Heading, string Body);

public record AgreementDetailResponse(
    Guid Id,
    int Version,
    bool GeneratedByAi,
    List<AgreementSectionResponse>? Sections,
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
