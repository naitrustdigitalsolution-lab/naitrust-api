namespace Naitrust.Domain.Models.Dtos.Responses.Verification;

public record FaceMatchResultResponse(bool Match, decimal MatchScore, decimal Confidence, bool LivenessPassed);
