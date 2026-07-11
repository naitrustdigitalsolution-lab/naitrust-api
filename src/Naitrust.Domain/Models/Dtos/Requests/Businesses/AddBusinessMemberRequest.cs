namespace Naitrust.Domain.Models.Dtos.Requests.Businesses;

public record AddBusinessMemberRequest(Guid UserId, string Role);
