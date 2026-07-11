namespace Naitrust.Domain.Models.Dtos.Requests.Evidence;

public record UploadEvidenceRequest(Guid TransactionId, Guid? MilestoneId, string Type, string? Description);
