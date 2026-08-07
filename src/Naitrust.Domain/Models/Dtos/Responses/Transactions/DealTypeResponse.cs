namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealTypeResponse(
    Guid Id,
    string Key,
    string Name,
    string RequiredVerificationLevel,
    string ReleaseMode,
    int AutoConfirmWindowHours,
    bool IsActive);
