using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Application.ExternalServices;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Upload;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IStorageService _storage;

    public UploadController(IStorageService storage)
    {
        _storage = storage;
    }

    /// <summary>Upload a verification document and receive a URL reference</summary>
    [HttpPost("verification-document")]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<UploadResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            var bad = NaitrustResponse<UploadResponse>.BadRequest("No file provided.");
            return StatusCode((int)bad.StatusCode, bad);
        }

        await using var stream = file.OpenReadStream();
        var url = await _storage.UploadFileAsync(stream, file.FileName, file.ContentType, ct);

        var fileId = Path.GetFileNameWithoutExtension(url.Split('/').Last());
        var response = NaitrustResponse<UploadResponse>.Success("File uploaded successfully.", new UploadResponse(url, fileId));
        return StatusCode((int)response.StatusCode, response);
    }
}
