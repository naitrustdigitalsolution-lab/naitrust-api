namespace Naitrust.Domain.Models.Dtos.Requests.Payments;

public record CreateSettlementAccountRequest(string PartnerId, Guid? BusinessId);
