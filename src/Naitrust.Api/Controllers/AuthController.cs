using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Authenticate with email and password
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Log out and revoke all refresh tokens
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> LogoutAsync()
    {
        var response = await _authService.LogoutAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Get the current authenticated user's profile
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetMeAsync()
    {
        var response = await _authService.GetCurrentUserAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Verify email address with a confirmation token
    /// </summary>
    [HttpPost("verify-email")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailRequest request)
    {
        var response = await _authService.VerifyEmailAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Request a password reset email
    /// </summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
    {
        var response = await _authService.ForgotPasswordAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Reset password using a valid reset token
    /// </summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        var response = await _authService.ResetPasswordAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Refresh access token using a valid refresh token
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
