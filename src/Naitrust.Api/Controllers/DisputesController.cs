// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Naitrust.Domain.Models.Dtos.Common;
// using Naitrust.Domain.Models.Dtos.Requests.Disputes;
//
// namespace Naitrust.Api.Controllers;
//
// [ApiController]
// [Route("api")]
// [Authorize]
// public class DisputesController : ControllerBase
// {
//     [HttpPost("transactions/{id:guid}/disputes")]
//     public Task<IActionResult> OpenAsync(Guid id, [FromBody] OpenDisputeRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("transactions/{id:guid}/disputes")]
//     public Task<IActionResult> ListByTransactionAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpGet("disputes/{id:guid}")]
//     public Task<IActionResult> GetByIdAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("disputes/{id:guid}/messages")]
//     public Task<IActionResult> AddMessageAsync(Guid id, [FromBody] AddDisputeMessageRequest request)
//         => throw new NotImplementedException();
//
//     [HttpPost("disputes/{id:guid}/evidence")]
//     public Task<IActionResult> AddEvidenceAsync(Guid id, [FromBody] AddDisputeEvidenceRequest request)
//         => throw new NotImplementedException();
// }
