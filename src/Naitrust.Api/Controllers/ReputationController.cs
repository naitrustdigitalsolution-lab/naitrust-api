// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Naitrust.Domain.Models.Dtos.Common;
// using Naitrust.Domain.Models.Dtos.Requests.Reputation;
//
// namespace Naitrust.Api.Controllers;
//
// [ApiController]
// [Route("api/reputation")]
// [Authorize]
// public class ReputationController : ControllerBase
// {
//     [HttpGet("{profileId:guid}")]
//     public Task<IActionResult> GetProfileAsync(Guid profileId)
//         => throw new NotImplementedException();
//
//     [HttpGet("me")]
//     public Task<IActionResult> GetMyAsync()
//         => throw new NotImplementedException();
//
//     [HttpPost("~/api/transactions/{id:guid}/reviews")]
//     public Task<IActionResult> PostReviewAsync(Guid id, [FromBody] SubmitReviewRequest request)
//         => throw new NotImplementedException();
// }
