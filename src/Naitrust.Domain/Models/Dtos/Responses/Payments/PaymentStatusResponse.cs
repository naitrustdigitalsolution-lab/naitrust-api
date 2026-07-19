namespace Naitrust.Domain.Models.Dtos.Responses.Payments;

public record PaymentStatusResponse(
    Guid TransactionId,
    string PaymentStatus,
    long EscrowBalanceMinor,
    LedgerSummaryDto? LedgerSummary);

public record LedgerSummaryDto(long TotalDebitMinor, long TotalCreditMinor, string Currency);
