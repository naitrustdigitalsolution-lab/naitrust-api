using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Api.Controllers;

/// <summary>
/// Payment partner webhook endpoints (no auth — verified via signatures)
/// </summary>
[ApiController]
[Route("api/webhooks")]
public class WebhooksController : ControllerBase
{
    /// <summary>
    /// Handle funding confirmation from a payment partner
    /// </summary>
    [HttpPost("payment-partners/{partner}/funding")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public Task<IActionResult> PaymentFundingAsync(string partner)
        => throw new NotImplementedException();

    /// <summary>
    /// Handle transfer confirmation from a payment partner
    /// </summary>
    [HttpPost("payment-partners/{partner}/transfer")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    public Task<IActionResult> PaymentTransferAsync(string partner)
        => throw new NotImplementedException();
}
