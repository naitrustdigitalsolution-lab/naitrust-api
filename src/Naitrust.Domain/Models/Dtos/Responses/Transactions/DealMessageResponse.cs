namespace Naitrust.Domain.Models.Dtos.Responses.Transactions;

public record DealMessageResponse(
    Guid Id,
    Guid DealId,
    Guid SenderId,
    string SenderName,
    bool IsYou,
    string Body,
    DateTime CreatedAt);
