using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Verification;
using Naitrust.Domain.Models.Dtos.Responses.Verification;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/verification")]
[Authorize]
public class VerificationController : ControllerBase
{
    private readonly IVerificationService _verificationService;

    public VerificationController(IVerificationService verificationService)
    {
        _verificationService = verificationService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Start a new verification request</summary>
    [HttpPost("start")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> StartAsync([FromBody] StartVerificationRequest request)
    {
        var response = await _verificationService.StartVerificationAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get the current user's verification status</summary>
    [HttpGet("status")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetStatusAsync()
    {
        var response = await _verificationService.CheckReusableVerificationAsync(GetUserId(), "identity");
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit individual identity verification data</summary>
    [HttpPost("individual")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SubmitIndividualAsync([FromBody] IndividualVerificationRequest request)
    {
        var response = await _verificationService.SubmitIndividualAsync(Guid.Empty, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit business verification data</summary>
    [HttpPost("business")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SubmitBusinessAsync([FromBody] BusinessVerificationRequest request)
    {
        var response = await _verificationService.SubmitBusinessAsync(Guid.Empty, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit facial verification (liveness check)</summary>
    [HttpPost("{requestId:guid}/facial")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SubmitFacialAsync(Guid requestId, [FromBody] FacialVerificationRequest request)
    {
        var response = await _verificationService.SubmitFacialAsync(requestId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Upload verification documents</summary>
    [HttpPost("{requestId:guid}/documents")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UploadDocumentsAsync(Guid requestId, [FromBody] UploadVerificationDocumentRequest request)
    {
        var response = await _verificationService.UploadDocumentsAsync(requestId, GetUserId(), request, Stream.Null, string.Empty);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit ownership verification data</summary>
    [HttpPost("{requestId:guid}/ownership")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SubmitOwnershipAsync(Guid requestId, [FromBody] OwnershipVerificationRequest request)
    {
        var response = await _verificationService.SubmitOwnershipAsync(requestId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Verify a one-time code for a verification step</summary>
    [HttpPost("{requestId:guid}/verify-code")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> VerifyCodeAsync(Guid requestId, [FromBody] VerifyCodeRequest request)
    {
        var response = await _verificationService.VerifyCodeAsync(requestId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get a verification request by ID</summary>
    [HttpGet("requests/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetRequestAsync(Guid id)
    {
        var response = await _verificationService.GetRequestAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Trigger automated verification processing</summary>
    [HttpPost("requests/{id:guid}/run")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RunAsync(Guid id)
    {
        var response = await _verificationService.RunVerificationAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Request additional information for a verification step</summary>
    [HttpPost("requests/{id:guid}/request-more-info")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<VerificationRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RequestMoreInfoAsync(Guid id, [FromBody] RequestMoreInfoRequest request)
    {
        var response = await _verificationService.RequestMoreInfoAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }
}
