// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Naitrust.Domain.Models.Dtos.Common;
// using Naitrust.Domain.Models.Dtos.Requests.Businesses;
//
// namespace Naitrust.Api.Controllers;
//
// [ApiController]
// [Route("api/businesses")]
// [Authorize]
// public class BusinessesController : ControllerBase
// {
//     [HttpPost]
//     public Task<IActionResult> CreateAsync([FromBody] CreateBusinessRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("me")]
//     public Task<IActionResult> GetMyAsync()
//         => throw new NotImplementedException();
//
//     [HttpGet("{id:guid}")]
//     public Task<IActionResult> GetByIdAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPatch("{id:guid}")]
//     public Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBusinessRequest request)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/members")]
//     public Task<IActionResult> AddMemberAsync(Guid id, [FromBody] AddBusinessMemberRequest request)
//         => throw new NotImplementedException();
//
//     [HttpPatch("{id:guid}/members/{memberId:guid}")]
//     public Task<IActionResult> UpdateMemberAsync(Guid id, Guid memberId, [FromBody] UpdateBusinessMemberRequest request)
//         => throw new NotImplementedException();
// }
