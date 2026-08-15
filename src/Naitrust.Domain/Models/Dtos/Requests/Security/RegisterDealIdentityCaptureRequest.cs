namespace Naitrust.Domain.Models.Dtos.Requests.Security;

/// <summary>
/// "deal_created" | "deal_accepted" — only deal_created is wired end-to-end by the frontend today.
/// The photo itself is bound separately as an IFormFile controller parameter (Domain has no ASP.NET Core reference).
/// </summary>
public record RegisterDealIdentityCaptureRequest(
    Guid DealId,
    string RepresentativeName,
    string? BusinessName,
    string Action);
