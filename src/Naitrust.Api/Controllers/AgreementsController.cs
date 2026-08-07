using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Requests.Agreements;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/agreements")]
[Authorize]
public class AgreementsController : ControllerBase
{
    private readonly IAgreementService _agreementService;

    public AgreementsController(IAgreementService agreementService)
    {
        _agreementService = agreementService;
    }

    [HttpPost("draft")]
    public async Task<IActionResult> DraftAsync([FromBody] DraftAgreementRequest request)
    {
        var response = await _agreementService.DraftAgreementAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
