namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record TransactionResponse(
    Guid Id,
    string Reference,
    string Title,
    string? Description,
    string Category,
    long AmountMinor,
    long FeeMinor,
    string Currency,
    string Status,
    string PaymentStatus,
    string PartyMode,
    string? RiskLevel,
    List<TransactionPartyResponse>? Parties,
    AgreementResponse? Agreement,
    List<AllowedActionDto>? AllowedActions,
    DateTime CreatedAt);

public record AllowedActionDto(string Action, string Label, bool Enabled);
