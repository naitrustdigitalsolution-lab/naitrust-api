namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record CreateMilestoneRequest(string Title, string? Description, long AmountMinor, DateTime? DueAt);
