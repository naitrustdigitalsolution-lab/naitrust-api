namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record UpdateMilestoneRequest(string? Title, string? Description, long? AmountMinor, DateTime? DueAt);
