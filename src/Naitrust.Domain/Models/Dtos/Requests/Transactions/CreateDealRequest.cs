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
    AgreementInput? Agreement,
    /// <summary>Fixed amount for the first funding stage. Takes precedence over InitialPaymentPercentage.</summary>
    long? InitialPaymentMinor = null,
    /// <summary>"fixed" | "percentage" — how InitialPaymentMinor/InitialPaymentPercentage should be read.</summary>
    string? InitialPaymentMode = null,
    /// <summary>1-100. Used to compute the first-stage amount when InitialPaymentMode is "percentage".</summary>
    int? InitialPaymentPercentage = null,
    long? RemainingPaymentMinor = null,
    string? NextPaymentReleaseConditions = null);

public record ParticipantInput(
    string Name,
    string? Email,
    string? Phone,
    string? Identifier,
    string? ProfileId,
    long? AllocationMinor,
    List<PaymentAllocationInput>? PaymentAllocations = null);

public record PaymentAllocationInput(int Stage, long AmountMinor);

public record AgreementInput(int Version, bool GeneratedByAi, List<AgreementSectionInput> Sections);
