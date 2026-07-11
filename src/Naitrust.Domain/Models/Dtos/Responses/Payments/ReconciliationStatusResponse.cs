namespace Naitrust.Domain.Models.Dtos.Responses.Payments;

public record ReconciliationStatusResponse(
    Guid TransactionId,
    bool Matches,
    long LedgerBalance,
    long? PartnerBalance,
    DateTime? LastCheckedAt);
