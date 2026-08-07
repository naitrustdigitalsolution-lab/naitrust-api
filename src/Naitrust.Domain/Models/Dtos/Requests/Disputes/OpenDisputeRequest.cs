namespace Naitrust.Domain.Models.Dtos.Requests.Disputes;

/// <summary>
/// Frontend sends {reason, description} — transactionId comes from route.
/// </summary>
public record OpenDisputeRequest(string Reason, string? Description);
