namespace Naitrust.Domain.Models.Constants;

public static class SignalREvents
{
    public const string TransactionInvitationAccepted = "TransactionInvitationAccepted";
    public const string TermsApproved = "TermsApproved";
    public const string PaymentStatusUpdated = "PaymentStatusUpdated";
    public const string EvidenceUploaded = "EvidenceUploaded";
    public const string DisputeOpened = "DisputeOpened";
    public const string DisputeResolved = "DisputeResolved";
    public const string TransactionCompleted = "TransactionCompleted";
    public const string VerificationStepUpdated = "VerificationStepUpdated";
    public const string VerificationCompleted = "VerificationCompleted";
    public const string VerificationRejected = "VerificationRejected";
    public const string VerificationMoreInfoRequested = "VerificationMoreInfoRequested";
    public const string NotificationReceived = "NotificationReceived";
}
