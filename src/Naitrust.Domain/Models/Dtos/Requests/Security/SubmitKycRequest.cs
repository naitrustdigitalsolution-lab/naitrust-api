namespace Naitrust.Domain.Models.Dtos.Requests.Security;

/// <summary>
/// KYC submission. Set Kind = "individual" or "business".
/// Individual fields: FirstName, LastName, Bvn, Dob, Gender.
/// Business fields: BusinessName, RcNumber, RegistrationType, Industry,
///   BusinessAddress, DirectorFirstName, DirectorLastName, DirectorBvn, OwnerEmail, Documents.
/// </summary>
public record SubmitKycRequest(
    string Kind,

    // ── Individual ───────────────────────────────────────────────
    string? FirstName = null,
    string? LastName = null,
    string? Bvn = null,
    string? Dob = null,          // YYYY-MM-DD
    string? Gender = null,

    // ── Business ─────────────────────────────────────────────────
    string? BusinessName = null,
    string? RcNumber = null,
    string? RegistrationType = null,
    string? Industry = null,
    string? BusinessAddress = null,
    string? DirectorFirstName = null,
    string? DirectorLastName = null,
    string? DirectorBvn = null,
    string? OwnerEmail = null,
    string? Documents = null      // comma-separated file names
);
