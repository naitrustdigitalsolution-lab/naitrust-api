namespace Naitrust.Domain.Models.Dtos.Requests.Public;

public record JoinWaitlistRequest(string Name, string Email, string? Phone, string? Source);
