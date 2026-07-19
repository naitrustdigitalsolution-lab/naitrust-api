namespace Naitrust.Domain.Models.Dtos.Requests.Roles;

public record UpdateRoleClaimRequest(string Role, string ClaimType, string NewClaimType);
