// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Naitrust.Domain.Models.Dtos.Common;
// using Naitrust.Domain.Models.Dtos.Requests.Payments;
//
// namespace Naitrust.Api.Controllers;
//
// [ApiController]
// [Route("api")]
// [Authorize]
// public class PaymentsController : ControllerBase
// {
//     [HttpPost("transactions/{id:guid}/virtual-account")]
//     public Task<IActionResult> CreateVirtualAccountAsync(Guid id, [FromBody] CreateVirtualAccountRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("transactions/{id:guid}/payment-status")]
//     public Task<IActionResult> GetPaymentStatusAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("transactions/{id:guid}/request-release")]
//     public Task<IActionResult> RequestReleaseAsync(Guid id, [FromBody] RequestReleaseRequest request)
//         => throw new NotImplementedException();
//
//     [HttpGet("transactions/{id:guid}/ledger")]
//     public Task<IActionResult> GetLedgerAsync(Guid id, [FromQuery] PaginationRequest pagination)
//         => throw new NotImplementedException();
//
//     [HttpGet("transactions/{id:guid}/reconciliation-status")]
//     public Task<IActionResult> GetReconciliationStatusAsync(Guid id)
//         => throw new NotImplementedException();
//
//     [HttpPost("payment-partners/{partner}/validate-payout-account")]
//     public Task<IActionResult> ValidatePayoutAccountAsync(string partner, [FromBody] ValidatePayoutAccountRequest request)
//         => throw new NotImplementedException();
// }
