namespace Naitrust.Domain.Models.Dtos.Responses.Payments;

public record VirtualAccountResponse(
    Guid Id,
    string? AccountNumber,
    string? AccountName,
    string? BankName,
    string Status,
    long AmountExpectedMinor,
    long AmountReceivedMinor,
    string Currency,
    DateTime? ExpiresAt);
