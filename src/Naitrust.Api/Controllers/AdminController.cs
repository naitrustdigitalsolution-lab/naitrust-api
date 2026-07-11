// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Naitrust.Domain.Models.Dtos.Common;
// using Naitrust.Domain.Models.Dtos.Requests.Admin;
//
// namespace Naitrust.Api.Controllers;
//
// [ApiController]
// [Route("api/admin")]
// [Authorize(Policy = "RequireAdmin")]
// public class AdminController : ControllerBase
// {
//     [HttpGet("transactions")]
//     public Task<IActionResult> ListTransactionsAsync([FromQuery] PaginationRequest pagination)
//         => throw new NotImplementedException();
//
//     [HttpGet("transactions/{id:guid}")]
//     public Task<IActionResult> GetTransactionAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpGet("disputes")]
//     public Task<IActionResult> ListDisputesAsync([FromQuery] PaginationRequest pagination)
//         => throw new NotImplementedException();
//
//     [HttpPatch("disputes/{id:guid}")]
//     public Task<IActionResult> ResolveDisputeAsync(Guid id, [FromBody] ResolveAdminDisputeRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("verifications")]
//     public Task<IActionResult> ListVerificationsAsync([FromQuery] PaginationRequest pagination)
//         => throw new NotImplementedException();
//
//     [HttpPatch("verifications/{id:guid}")]
//     public Task<IActionResult> UpdateVerificationAsync(Guid id, [FromBody] UpdateAdminVerificationRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("audit-logs")]
//     public Task<IActionResult> GetAuditLogsAsync([FromQuery] PaginationRequest pagination)
//         => throw new NotImplementedException();
// }
