namespace Naitrust.Domain.Models.Constants;

public static class CacheKeys
{
    public static string RateLimit(string key) => $"rate_limit:{key}";
    public static string Idempotency(string key) => $"idempotency:{key}";
    public static string Session(string userId) => $"session:{userId}";
    public static string VerificationStatus(string subjectType, string subjectId) => $"verification:{subjectType}:{subjectId}";
}
