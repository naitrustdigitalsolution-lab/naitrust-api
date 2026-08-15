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
    string? ReleaseConditions,
    /// <summary>First-stage amount for a staged/split payment. Null (or equal to AmountMinor) for a single payment.</summary>
    long? InitialPaymentMinor = null,
    string? NextPaymentReleaseConditions = null,
    int? ExtendedProductTestingDays = null);
