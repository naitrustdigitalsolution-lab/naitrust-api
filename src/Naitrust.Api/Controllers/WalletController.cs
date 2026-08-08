using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Wallet;
using Naitrust.Domain.Models.Dtos.Responses.Wallet;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/wallet")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get the authenticated user's wallet with live balance</summary>
    [HttpGet("me")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<WalletAccountResponse>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> GetWalletAsync()
    {
        var response = await _walletService.GetWalletAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>List all linked bank accounts for withdrawal</summary>
    [HttpGet("bank-accounts")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<LinkedBankAccountResponse>>))]
    public async Task<IActionResult> GetBankAccountsAsync()
    {
        var response = await _walletService.GetLinkedBankAccountsAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Add and validate a new bank account for withdrawal</summary>
    [HttpPost("bank-accounts")]
    [ProducesResponseType(201, Type = typeof(NaitrustResponse<LinkedBankAccountResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(409, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> AddBankAccountAsync([FromBody] AddLinkedBankAccountRequest request)
    {
        var response = await _walletService.AddLinkedBankAccountAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Set a linked bank account as the default for withdrawal</summary>
    [HttpPatch("bank-accounts/{id:guid}/set-default")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> SetDefaultBankAccountAsync(Guid id)
    {
        var response = await _walletService.SetDefaultBankAccountAsync(GetUserId(), id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Remove a linked bank account</summary>
    [HttpDelete("bank-accounts/{id:guid}")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<bool>))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> RemoveBankAccountAsync(Guid id)
    {
        var response = await _walletService.RemoveLinkedBankAccountAsync(GetUserId(), id);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Withdraw funds from wallet to a linked bank account via NIP transfer</summary>
    [HttpPost("withdraw")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<WalletAccountResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(404, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> WithdrawAsync([FromBody] WithdrawRequest request)
    {
        var response = await _walletService.WithdrawAsync(GetUserId(), request);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>Get wallet activity feed (withdrawals, deposits, deal movements)</summary>
    [HttpGet("activity")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<List<WalletActivityResponse>>))]
    public async Task<IActionResult> GetActivityAsync()
    {
        var response = await _walletService.GetActivityAsync(GetUserId());
        return StatusCode((int)response.StatusCode, response);
    }
}
