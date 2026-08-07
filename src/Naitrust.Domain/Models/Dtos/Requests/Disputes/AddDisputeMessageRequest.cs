namespace Naitrust.Domain.Models.Dtos.Requests.Disputes;

/// <summary>
/// Frontend sends {body} for dispute messages.
/// </summary>
public record AddDisputeMessageRequest(string Body);
