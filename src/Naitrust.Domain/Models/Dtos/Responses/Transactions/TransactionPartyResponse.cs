namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record TransactionPartyResponse(
    Guid Id,
    Guid? UserId,
    Guid? BusinessId,
    string PartyType,
    string DisplayName,
    string? Email,
    string Status,
    DateTime? AcceptedAt);
