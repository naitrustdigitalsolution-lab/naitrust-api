namespace Naitrust.Domain.Models.Dtos.Requests.Verification;

public record BusinessVerificationRequest(
    string RegistrationType,
    string RegistrationNumber,
    string BusinessName,
    string? Tin,
    string? Address,
    string? OwnerFirstName,
    string? OwnerLastName,
    string? OwnerIdType,
    string? OwnerIdNumber);
