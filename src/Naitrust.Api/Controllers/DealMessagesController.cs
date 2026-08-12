using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/transactions/{txnId:guid}/messages")]
[Authorize]
public class DealMessagesController : ControllerBase
{
    private readonly IDealSubResourceService _service;

    public DealMessagesController(IDealSubResourceService service)
    {
        _service = service;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>List all messages in a transaction thread</summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<DealMessageResponse>>))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> ListAsync(Guid txnId)
    {
        var response = await _service.GetMessagesAsync(txnId, GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Send a message in a transaction thread</summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<DealMessageResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SendAsync(Guid txnId, [FromBody] SendDealMessageRequest request)
    {
        var response = await _service.SendMessageAsync(txnId, GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }
}
