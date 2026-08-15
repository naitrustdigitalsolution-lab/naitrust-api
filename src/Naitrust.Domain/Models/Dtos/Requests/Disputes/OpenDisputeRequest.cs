namespace Naitrust.Domain.Models.Dtos.Requests.Disputes;

/// <summary>
/// Frontend sends {reason, description, hasEvidence} — transactionId comes from route.
/// HasEvidence decides whether the dispute opens straight into review or waits on evidence.
/// </summary>
public record OpenDisputeRequest(string Reason, string? Description, bool HasEvidence = false);
