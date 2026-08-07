namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record AddTrackingStepRequest(string Title, string? Description, Guid? AfterStepId);
