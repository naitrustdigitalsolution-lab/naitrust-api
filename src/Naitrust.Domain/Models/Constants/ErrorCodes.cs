namespace Naitrust.Domain.Models.Constants;

public static class ErrorCodes
{
    public const string VALIDATION_ERROR = "VALIDATION_ERROR";
    public const string UNAUTHORIZED = "UNAUTHORIZED";
    public const string FORBIDDEN = "FORBIDDEN";
    public const string NOT_FOUND = "NOT_FOUND";
    public const string CONFLICT = "CONFLICT";
    public const string PAYMENT_FAILED = "PAYMENT_FAILED";
    public const string VERIFICATION_REQUIRED = "VERIFICATION_REQUIRED";
    public const string INSUFFICIENT_VERIFICATION = "INSUFFICIENT_VERIFICATION";
    public const string LIVENESS_REQUIRED = "LIVENESS_REQUIRED";
    public const string DISPUTE_ACTIVE = "DISPUTE_ACTIVE";
    public const string RELEASE_BLOCKED = "RELEASE_BLOCKED";
    public const string DUPLICATE_REQUEST = "DUPLICATE_REQUEST";
    public const string RATE_LIMITED = "RATE_LIMITED";
    public const string INTERNAL_ERROR = "INTERNAL_ERROR";
    public const string PARTNER_ERROR = "PARTNER_ERROR";
    public const string WEBHOOK_INVALID = "WEBHOOK_INVALID";
    public const string NAME_MISMATCH = "NAME_MISMATCH";
    public const string ACCOUNT_SUSPENDED = "ACCOUNT_SUSPENDED";
}
