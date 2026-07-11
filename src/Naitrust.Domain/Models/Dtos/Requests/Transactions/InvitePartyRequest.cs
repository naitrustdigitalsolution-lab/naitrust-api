namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record InvitePartyRequest(string? Email, string? Phone, string PartyType, string? DisplayName);
