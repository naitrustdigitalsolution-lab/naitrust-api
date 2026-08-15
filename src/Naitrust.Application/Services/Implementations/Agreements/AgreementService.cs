using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Agreements;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Application.Services.Implementations.Agreements;

public class AgreementService : IAgreementService
{
    public Task<NaitrustResponse<AgreementResponse>> DraftAgreementAsync(DraftAgreementRequest request, CancellationToken ct = default)
    {
        var amountFormatted = $"{request.Currency} {request.AmountMinor / 100m:N2}";
        var deliveryDue = !string.IsNullOrWhiteSpace(request.DeliveryDueDate)
            ? request.DeliveryDueDate
            : "as agreed by both parties";
        var releaseConditions = !string.IsNullOrWhiteSpace(request.ReleaseConditions)
            ? request.ReleaseConditions
            : "upon buyer confirmation of satisfactory delivery";
        var reviewWindow = request.ExtendedProductTestingDays is > 0
            ? $"extended {request.ExtendedProductTestingDays}-day product-testing period"
            : "standard 1-hour payment-review period";

        var hasStagedPayment = request.InitialPaymentMinor is > 0 && request.InitialPaymentMinor < request.AmountMinor;
        var initialAmountFormatted = hasStagedPayment ? $"{request.Currency} {request.InitialPaymentMinor!.Value / 100m:N2}" : null;
        var remainingAmountFormatted = hasStagedPayment ? $"{request.Currency} {(request.AmountMinor - request.InitialPaymentMinor!.Value) / 100m:N2}" : null;
        var nextPaymentCondition = !string.IsNullOrWhiteSpace(request.NextPaymentReleaseConditions)
            ? request.NextPaymentReleaseConditions
            : "the agreed next-stage condition";

        var sections = new List<AgreementSectionResponse>
        {
            new("Parties & Purpose",
                $"This agreement is between {request.BuyerName} (\"Buyer\") and {request.SellerName} (\"Seller\") " +
                $"for: {request.Title}." +
                (!string.IsNullOrWhiteSpace(request.Description) ? $" {request.Description}" : "")),

            new("Protected Payment",
                hasStagedPayment
                    ? $"The total deal value is {amountFormatted}. The Buyer shall first deposit {initialAmountFormatted} into a Naitrust escrow account. " +
                      $"Naitrust will track the remaining {remainingAmountFormatted}, which becomes due only after this condition is confirmed: {nextPaymentCondition}"
                    : $"The Buyer shall deposit {amountFormatted} into a Naitrust escrow account. " +
                      "Funds are held securely by Naitrust and cannot be accessed by either party until release conditions are met."),

            new("Delivery Obligations",
                $"The Seller shall deliver the goods or services as described above by {deliveryDue}. " +
                "The Seller must provide evidence of delivery as specified in the transaction."),

            new("Release Conditions",
                hasStagedPayment
                    ? $"The first payment releases to the Seller {releaseConditions}, followed by a {reviewWindow}. " +
                      $"The remaining payment stays locked until the first payment has released successfully, then requires this condition: {nextPaymentCondition} Each release has its own review period."
                    : $"Escrow funds shall be released to the Seller {releaseConditions}, followed by a {reviewWindow}. " +
                      "If no action is taken within the auto-confirm window, funds may be released automatically."),

            new("Disputes",
                "Either party may raise a dispute through the Naitrust platform if they believe the other party " +
                "has not met their obligations. Naitrust will mediate disputes based on evidence provided by both parties."),

            new("Acceptance",
                "By proceeding with this transaction, both parties acknowledge and accept these terms. " +
                "This agreement is binding once both parties have confirmed acceptance on the Naitrust platform.")
        };

        var response = new AgreementResponse(
            Id: Guid.Empty,
            Version: 1,
            GeneratedByAi: false,
            Sections: sections);

        return Task.FromResult(NaitrustResponse<AgreementResponse>.Success("Agreement draft generated.", response));
    }
}
