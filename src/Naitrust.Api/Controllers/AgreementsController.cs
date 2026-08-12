using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Agreements;
using Naitrust.Domain.Models.Dtos.Responses.Transactions;

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

    /// <summary>Draft a new agreement document for a deal</summary>
    [HttpPost("draft")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<AgreementResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> DraftAsync([FromBody] DraftAgreementRequest request)
    {
        var response = await _agreementService.DraftAgreementAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }
}
