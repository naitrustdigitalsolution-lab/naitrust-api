using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Security;
using Naitrust.Domain.Models.Dtos.Responses.Security;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/security")]
[Authorize]
public class SecurityController : ControllerBase
{
    private readonly ISecurityService _securityService;

    public SecurityController(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ── Email ──────────────────────────────────────────────────────

    /// <summary>Send an OTP to the given email address for verification.</summary>
    [HttpPost("email/send-otp")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SendEmailOtpAsync([FromBody] SendEmailOtpRequest request)
    {
        var response = await _securityService.SendEmailOtpAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Verify the email OTP and mark the email address as confirmed.</summary>
    [HttpPost("email/verify")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> VerifyEmailOtpAsync([FromBody] VerifyEmailOtpRequest request)
    {
        var response = await _securityService.VerifyEmailOtpAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    // ── Phone ──────────────────────────────────────────────────────

    /// <summary>Send an OTP via SMS to the given phone number for verification.</summary>
    [HttpPost("phone/send-otp")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SendPhoneOtpAsync([FromBody] SendPhoneOtpRequest request)
    {
        var response = await _securityService.SendPhoneOtpAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Verify the phone OTP and mark the phone number as confirmed.</summary>
    [HttpPost("phone/verify")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> VerifyPhoneOtpAsync([FromBody] VerifyPhoneOtpRequest request)
    {
        var response = await _securityService.VerifyPhoneOtpAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    // ── 2FA ───────────────────────────────────────────────────────

    /// <summary>Generate a TOTP secret and return it with an otpauth URI for QR code display.</summary>
    [HttpPost("2fa/start")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<Start2faSetupResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> Start2faAsync([FromBody] Start2faRequest request)
    {
        var response = await _securityService.Start2faAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Verify the TOTP code from the authenticator app and enable 2FA.</summary>
    [HttpPost("2fa/verify")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> Verify2faSetupAsync([FromBody] Verify2faSetupRequest request)
    {
        var response = await _securityService.Verify2faSetupAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    // ── KYC ───────────────────────────────────────────────────────

    /// <summary>Submit a KYC application (individual or business).</summary>
    [HttpPost("kyc")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SubmitKycAsync([FromBody] SubmitKycRequest request)
    {
        var response = await _securityService.SubmitKycAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    // ── PIN ───────────────────────────────────────────────────────

    /// <summary>Set or reset the user's transaction PIN.</summary>
    [HttpPost("pin/set")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SetPinAsync([FromBody] SetPinRequest request)
    {
        var response = await _securityService.SetPinAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Verify the user's transaction PIN (used before sensitive operations).</summary>
    [HttpPost("pin/verify")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> VerifyPinAsync([FromBody] VerifyPinRequest request)
    {
        var response = await _securityService.VerifyPinAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
