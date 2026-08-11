using Newtonsoft.Json;

namespace Naitrust.Application.ExternalServices.QoreId;

// ── Token ─────────────────────────────────────────────────────────────────────

internal record QoreIdTokenRequest(
    [property: JsonProperty("clientId")] string ClientId,
    [property: JsonProperty("secret")]   string Secret);

internal record QoreIdTokenResponse(
    [property: JsonProperty("accessToken")] string AccessToken,
    [property: JsonProperty("expiresIn")]   string ExpiresIn,
    [property: JsonProperty("tokenType")]   string TokenType);

// ── BVN ───────────────────────────────────────────────────────────────────────

public record QoreIdBvnRequest(
    string BvnNumber,
    string FirstName,
    string LastName,
    string? Dob    = null,
    string? Gender = null);

public record QoreIdBvnResult(
    bool    Verified,
    string? FirstName,
    string? LastName,
    string? DateOfBirth,
    string? Phone,
    string? ErrorMessage);

internal record QoreIdBvnRequestBody(
    [property: JsonProperty("firstname")] string  Firstname,
    [property: JsonProperty("lastname")]  string  Lastname,
    [property: JsonProperty("dob")]       string? Dob,
    [property: JsonProperty("gender")]    string? Gender);

internal record QoreIdBvnApiResponse(
    [property: JsonProperty("status")]  QoreIdStatus?  Status,
    [property: JsonProperty("summary")] QoreIdSummary? Summary,
    [property: JsonProperty("bvn")]     QoreIdBvnData? Bvn);

// ── CAC ───────────────────────────────────────────────────────────────────────

public record QoreIdCacRequest(string RegNumber);

public record QoreIdCacResult(
    bool    Verified,
    string? CompanyName,
    string? CompanyStatus,
    string? ErrorMessage);

internal record QoreIdCacRequestBody(
    [property: JsonProperty("regNumber")] string RegNumber);

internal record QoreIdCacApiResponse(
    [property: JsonProperty("status")]  QoreIdStatus?  Status,
    [property: JsonProperty("summary")] QoreIdSummary? Summary,
    [property: JsonProperty("cac")]     QoreIdCacData? Cac);

// ── Shared response shapes ────────────────────────────────────────────────────

internal record QoreIdStatus(
    [property: JsonProperty("state")]  string? State,
    [property: JsonProperty("status")] string? StatusValue);

internal record QoreIdSummary(
    [property: JsonProperty("bvn_check")] QoreIdBvnCheck? BvnCheck,
    [property: JsonProperty("cac_check")] string?         CacCheck);

internal record QoreIdBvnCheck(
    [property: JsonProperty("status")]      string?            Status,
    [property: JsonProperty("fieldMatches")] QoreIdFieldMatches? FieldMatches);

internal record QoreIdFieldMatches(
    [property: JsonProperty("firstname")] bool Firstname,
    [property: JsonProperty("lastname")]  bool Lastname);

internal record QoreIdBvnData(
    [property: JsonProperty("bvn")]       string? Bvn,
    [property: JsonProperty("firstname")] string? Firstname,
    [property: JsonProperty("lastname")]  string? Lastname,
    [property: JsonProperty("birthdate")] string? Birthdate,
    [property: JsonProperty("gender")]    string? Gender,
    [property: JsonProperty("phone")]     string? Phone,
    [property: JsonProperty("photo")]     string? Photo);

internal record QoreIdCacData(
    [property: JsonProperty("companyName")]       string? CompanyName,
    [property: JsonProperty("status")]            string? Status,
    [property: JsonProperty("rcNumber")]          string? RcNumber,
    [property: JsonProperty("companyType")]       string? CompanyType,
    [property: JsonProperty("registrationDate")]  string? RegistrationDate);
