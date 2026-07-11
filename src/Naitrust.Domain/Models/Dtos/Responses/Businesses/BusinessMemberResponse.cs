namespace Naitrust.Domain.Models.Dtos.Responses.Businesses;

public record BusinessMemberResponse(Guid Id, Guid UserId, string Role, string Status, DateTime CreatedAt);
