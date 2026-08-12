using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.PaymentRequests;
using Naitrust.Domain.Models.Dtos.Responses.PaymentRequests;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/payment-requests")]
[Authorize]
public class PaymentRequestsController : ControllerBase
{
    private readonly IPaymentRequestService _service;

    public PaymentRequestsController(IPaymentRequestService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>List all payment requests created by the authenticated user</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<PaymentRequestResponse>>))]
    public async Task<IActionResult> ListAsync()
    {
        var response = await _service.ListAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Create a new payment request</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<PaymentRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentRequestRequest request)
    {
        var response = await _service.CreateAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Respond to a payment request (fulfil or decline)</summary>
    [HttpPost("{id:guid}/respond")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<PaymentRequestResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RespondAsync(Guid id, [FromBody] RespondPaymentRequestRequest request)
    {
        var response = await _service.RespondAsync(GetUserId(), id, request);
        return StatusCode((int)response.StatusCode, response);
    }
}
