namespace Naitrust.Domain.Models.Dtos.Requests.Agreements;

public record DraftAgreementRequest(
    string UseCaseTitle,
    string BuyerName,
    string SellerName,
    string Title,
    string? Description,
    long AmountMinor,
    string Currency,
    string? DeliveryDueDate,
    string? ReleaseConditions);
