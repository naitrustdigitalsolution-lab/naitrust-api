namespace Naitrust.Domain.Models.Dtos.Requests.Payments;

public record CreateVirtualAccountRequest(Guid TransactionId, string PartnerId);
