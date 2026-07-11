namespace Naitrust.Application.ExternalServices;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);
    Task DeleteFileAsync(string fileUrl, CancellationToken ct = default);
    Task<string> GetFileUrlAsync(string fileKey, CancellationToken ct = default);
}
