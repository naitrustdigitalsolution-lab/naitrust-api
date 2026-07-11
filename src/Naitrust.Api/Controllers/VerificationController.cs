using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Verification;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/verification")]
[Authorize]
public class VerificationController : ControllerBase
{
    [HttpPost("start")]
    public Task<IActionResult> StartAsync([FromBody] StartVerificationRequest request)
        => throw new NotImplementedException();

    [HttpGet("status")]
    public Task<IActionResult> GetStatusAsync()
        => throw new NotImplementedException();

    [HttpPost("individual")]
    public Task<IActionResult> SubmitIndividualAsync([FromBody] IndividualVerificationRequest request)
        => throw new NotImplementedException();

    [HttpPost("business")]
    public Task<IActionResult> SubmitBusinessAsync([FromBody] BusinessVerificationRequest request)
        => throw new NotImplementedException();

    [HttpPost("{requestId:guid}/facial")]
    public Task<IActionResult> SubmitFacialAsync(Guid requestId)
        => throw new NotImplementedException();

    [HttpPost("{requestId:guid}/documents")]
    public Task<IActionResult> UploadDocumentsAsync(Guid requestId)
        => throw new NotImplementedException();

    [HttpPost("{requestId:guid}/ownership")]
    public Task<IActionResult> SubmitOwnershipAsync(Guid requestId, [FromBody] OwnershipVerificationRequest request)
        => throw new NotImplementedException();

    [HttpPost("{requestId:guid}/verify-code")]
    public Task<IActionResult> VerifyCodeAsync(Guid requestId, [FromBody] VerifyCodeRequest request)
        => throw new NotImplementedException();

    [HttpGet("requests/{id:guid}")]
    public Task<IActionResult> GetRequestAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("requests/{id:guid}/run")]
    public Task<IActionResult> RunAsync(Guid id)
        => throw new NotImplementedException();

    [HttpPost("requests/{id:guid}/request-more-info")]
    public Task<IActionResult> RequestMoreInfoAsync(Guid id, [FromBody] RequestMoreInfoRequest request)
        => throw new NotImplementedException();
}
