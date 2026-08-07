namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealResponse(
    Guid Id,
    string Reference,
    string Title,
    string? Description,
    string? UseCase,
    string DealType,
    string Category,
    long AmountMinor,
    long FeeMinor,
    string Currency,
    string Status,
    string PaymentStatus,
    string PartyMode,
    string? RiskLevel,
    string? DeliveryDueDate,
    string? ReleaseConditions,
    int? ExtendedProductTestingDays,
    DateTime? ExpiresAt,
    bool Recurring,
    string? PreviousReference,
    List<DealPartyResponse>? Parties,
    AgreementResponse? Agreement,
    List<AllowedActionDto>? AllowedActions,
    string? PublicInvitePath,
    DateTime CreatedAt);

public record AllowedActionDto(string Action, string Label, bool Enabled);
