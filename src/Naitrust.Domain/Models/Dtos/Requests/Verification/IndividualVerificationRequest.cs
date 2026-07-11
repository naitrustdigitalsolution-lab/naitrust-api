namespace Naitrust.Domain.Models.Dtos.Requests.Verification;

public record IndividualVerificationRequest(
    string IdType,
    string IdNumber,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth,
    string? Phone);
