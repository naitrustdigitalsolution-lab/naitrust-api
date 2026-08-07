namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealPartyResponse(
    Guid Id,
    Guid? UserId,
    Guid? BusinessId,
    string PartyType,
    string DisplayName,
    string? Email,
    string Status,
    DateTime? AcceptedAt,
    string? CounterpartyName = null,
    bool IsYou = false);
