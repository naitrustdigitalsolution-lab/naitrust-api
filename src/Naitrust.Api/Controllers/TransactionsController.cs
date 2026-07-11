// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Naitrust.Domain.Models.Dtos.Common;
// using Naitrust.Domain.Models.Dtos.Requests.Transactions;
//
// namespace Naitrust.Api.Controllers;
//
// [ApiController]
// [Route("api/transactions")]
// [Authorize]
// public class TransactionsController : ControllerBase
// {
//     [HttpPost]
//     public Task<IActionResult> CreateAsync([FromBody] CreateTransactionRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet]
//     public Task<IActionResult> ListAsync([FromQuery] PaginationRequest pagination)
//         => throw new NotImplementedException();
//
//     [HttpGet("{id:guid}")]
//     public Task<IActionResult> GetByIdAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPatch("{id:guid}")]
//     public Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateTransactionRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("~/api/transaction-types")]
//     public Task<IActionResult> GetTypesAsync()
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/invite")]
//     public Task<IActionResult> InviteAsync(Guid id, [FromBody] InvitePartyRequest request)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/accept")]
//     public Task<IActionResult> AcceptAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/reject")]
//     public Task<IActionResult> RejectAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/terms")]
//     public Task<IActionResult> ProposeTermsAsync(Guid id, [FromBody] ProposeTermsRequest request)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/approve-terms")]
//     public Task<IActionResult> ApproveTermsAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/fund")]
//     public Task<IActionResult> FundAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/deliver")]
//     public Task<IActionResult> DeliverAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/confirm")]
//     public Task<IActionResult> ConfirmAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("{id:guid}/cancel")]
//     public Task<IActionResult> CancelAsync(Guid id)
//         => throw new NotImplementedException();
// }
