using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Public;
using Naitrust.Domain.Models.Dtos.Responses.Public;

namespace Naitrust.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PublicController : ControllerBase
{
    private readonly IPublicFormService _publicFormService;

    public PublicController(IPublicFormService publicFormService)
    {
        _publicFormService = publicFormService;
    }

    /// <summary>Join the waitlist with name and email</summary>
    [HttpPost("joinWaitlist")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<PublicSubmissionResponse>))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> JoinWaitlist([FromBody] JoinWaitlistRequest request)
    {
        var response = await _publicFormService.JoinWaitlistAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit a contact us message</summary>
    [HttpPost("contactUs")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<PublicSubmissionResponse>))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ContactUs([FromBody] ContactUsRequest request)
    {
        var response = await _publicFormService.ContactUsAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Subscribe to the newsletter</summary>
    [HttpPost("subscribe")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<PublicSubmissionResponse>))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
    {
        var response = await _publicFormService.SubscribeAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Submit feedback with a rating</summary>
    [HttpPost("submitFeedback")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<PublicSubmissionResponse>))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SubmitFeedback([FromBody] SubmitFeedbackRequest request)
    {
        var response = await _publicFormService.SubmitFeedbackAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Report a concern or issue</summary>
    [HttpPost("reportConcern")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<PublicSubmissionResponse>))]
    [ProducesResponseType(422, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ReportConcern([FromBody] ReportConcernRequest request)
    {
        var response = await _publicFormService.ReportConcernAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
