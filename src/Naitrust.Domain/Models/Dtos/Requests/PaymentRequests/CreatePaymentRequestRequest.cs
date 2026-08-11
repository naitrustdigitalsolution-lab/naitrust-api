namespace Naitrust.Domain.Models.Dtos.Requests.PaymentRequests;

public record CreatePaymentRequestRequest(
    string RequestedFromName,
    long AmountMinor,
    string Currency,
    string? Reason = null);

public record RespondPaymentRequestRequest(string Action); // fulfil | decline
