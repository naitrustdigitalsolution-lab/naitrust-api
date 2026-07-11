namespace Naitrust.Domain.Models.Dtos.Responses.Payments;

public record PaymentStatusResponse(
    Guid TransactionId,
    string PaymentStatus,
    VirtualAccountResponse? VirtualAccount,
    LedgerSummaryDto? LedgerSummary);

public record LedgerSummaryDto(long TotalDebitMinor, long TotalCreditMinor, string Currency);
