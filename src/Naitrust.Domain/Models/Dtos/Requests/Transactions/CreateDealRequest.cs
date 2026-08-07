namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record CreateDealRequest(
    string UseCase,
    string DealType,
    string? PartyMode,
    string Role,
    List<ParticipantInput>? Participants,
    string Title,
    string? Description,
    long AmountMinor,
    string Currency,
    string? DeliveryDueDate,
    string? ReleaseConditions,
    int? ExtendedProductTestingDays,
    int? ExpiresInDays,
    AgreementInput? Agreement);

public record ParticipantInput(
    string Name,
    string? Email,
    string? Phone,
    string? Identifier,
    string? ProfileId,
    long? AllocationMinor);

public record AgreementInput(int Version, bool GeneratedByAi, List<AgreementSectionInput> Sections);
