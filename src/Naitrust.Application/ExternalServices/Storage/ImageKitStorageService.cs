namespace Naitrust.Application.ExternalServices.Storage;

public class ImageKitStorageService : IStorageService
{
    public Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task DeleteFileAsync(string fileUrl, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<string> GetFileUrlAsync(string fileKey, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
