using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Auth;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        => throw new NotImplementedException();

    [HttpPost("login")]
    public Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        => throw new NotImplementedException();

    [HttpPost("logout")]
    [Authorize]
    public Task<IActionResult> LogoutAsync()
        => throw new NotImplementedException();

    [HttpGet("me")]
    [Authorize]
    public Task<IActionResult> GetMeAsync()
        => throw new NotImplementedException();

    [HttpPost("verify-email")]
    public Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailRequest request)
        => throw new NotImplementedException();

    [HttpPost("forgot-password")]
    public Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        => throw new NotImplementedException();

    [HttpPost("reset-password")]
    public Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        => throw new NotImplementedException();
}
