namespace Naitrust.Domain.Models.Dtos.Requests.Transactions;

/// <summary>The buyer submits whichever credential they read off the delivery card — QR token or the OTP digits.</summary>
public record ConfirmDeliveryReceiptRequest(string? Token, string? OtpCode);
