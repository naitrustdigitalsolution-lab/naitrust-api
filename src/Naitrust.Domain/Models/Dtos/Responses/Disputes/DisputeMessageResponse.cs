namespace Naitrust.Domain.Models.Dtos.Responses.Disputes;

public record DisputeMessageResponse(Guid Id, Guid SenderUserId, string Message, DateTime CreatedAt);
