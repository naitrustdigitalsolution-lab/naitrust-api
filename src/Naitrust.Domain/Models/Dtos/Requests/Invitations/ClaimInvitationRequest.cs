namespace Naitrust.Domain.Models.Dtos.Requests.Invitations;

public record ClaimInvitationRequest(Guid UserId, string Email, string Role, bool KycVerified);
