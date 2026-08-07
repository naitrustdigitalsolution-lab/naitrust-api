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

        var sections = new List<AgreementSectionResponse>
        {
            new("Parties & Purpose",
                $"This agreement is between {request.BuyerName} (\"Buyer\") and {request.SellerName} (\"Seller\") " +
                $"for: {request.Title}." +
                (!string.IsNullOrWhiteSpace(request.Description) ? $" {request.Description}" : "")),

            new("Protected Payment",
                $"The Buyer shall deposit {amountFormatted} into a Naitrust escrow account. " +
                "Funds are held securely by Naitrust and cannot be accessed by either party until release conditions are met."),

            new("Delivery Obligations",
                $"The Seller shall deliver the goods or services as described above by {deliveryDue}. " +
                "The Seller must provide evidence of delivery as specified in the transaction."),

            new("Release Conditions",
                $"Escrow funds shall be released to the Seller {releaseConditions}. " +
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
