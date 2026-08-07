namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

public record RespondTerminationRequest(bool Accept, string? Reason);
