using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Responses.Upload;

namespace Naitrust.Api.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    /// <summary>Upload a file and receive a URL reference</summary>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(NaitrustResponse<UploadResponse>))]
    [ProducesResponseType(400, Type = typeof(NaitrustResponse))]
    [ProducesResponseType(401, Type = typeof(NaitrustResponse))]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            var badResponse = NaitrustResponse<UploadResponse>.BadRequest("No file provided.");
            return StatusCode((int)badResponse.StatusCode, badResponse);
        }

        // TODO: Replace with cloud storage (S3, Azure Blob, etc.)
        var fileId = Guid.NewGuid().ToString("N");
        var uploadsDir = Path.Combine(Path.GetTempPath(), "naitrust-uploads");
        Directory.CreateDirectory(uploadsDir);

        var extension = Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsDir, $"{fileId}{extension}");

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var url = $"/uploads/{fileId}{extension}";
        var response = NaitrustResponse<UploadResponse>.Success("File uploaded successfully.", new UploadResponse(url, fileId));
        return StatusCode((int)response.StatusCode, response);
    }
}
