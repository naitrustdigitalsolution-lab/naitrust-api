using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhooksController : ControllerBase
{
    [HttpPost("payment-partners/{partner}/funding")]
    public Task<IActionResult> PaymentFundingAsync(string partner)
        => throw new NotImplementedException();

    [HttpPost("payment-partners/{partner}/transfer")]
    public Task<IActionResult> PaymentTransferAsync(string partner)
        => throw new NotImplementedException();
}
