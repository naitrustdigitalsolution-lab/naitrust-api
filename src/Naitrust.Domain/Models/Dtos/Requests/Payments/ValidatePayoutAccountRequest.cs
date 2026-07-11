namespace Naitrust.Domain.Models.Dtos.Requests.Payments;

public record ValidatePayoutAccountRequest(string BankCode, string AccountNumber);
