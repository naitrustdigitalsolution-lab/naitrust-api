using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Naitrust.Domain.Configurations.ConfigModels;

namespace Naitrust.Application.ExternalServices.Storage;

/// <summary>
/// S3-compatible storage service — works with Railway (MinIO), AWS S3, or any S3-compatible provider.
/// Configure via appsettings: Storage.ServiceUrl, Storage.BucketName, Storage.AccessKey, Storage.SecretKey.
/// </summary>
public class S3StorageService : IStorageService
{
    private readonly AmazonS3Client _client;
    private readonly StorageSettings _settings;

    public S3StorageService(IOptions<StorageSettings> options)
    {
        _settings = options.Value;

        var config = new AmazonS3Config
        {
            ServiceURL = _settings.ServiceUrl,
            ForcePathStyle = true // Required for MinIO / non-AWS S3-compatible providers
        };

        _client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
    }

    /// <summary>Uploads a file and returns the public URL.</summary>
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        var key = $"{Guid.NewGuid():N}/{SanitizeFileName(fileName)}";

        var request = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };

        await _client.PutObjectAsync(request, ct);

        return BuildPublicUrl(key);
    }

    /// <summary>Deletes a file by its public URL.</summary>
    public async Task DeleteFileAsync(string fileUrl, CancellationToken ct = default)
    {
        var key = ExtractKeyFromUrl(fileUrl);

        var request = new DeleteObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key
        };

        await _client.DeleteObjectAsync(request, ct);
    }

    /// <summary>Returns the public URL for a given file key.</summary>
    public Task<string> GetFileUrlAsync(string fileKey, CancellationToken ct = default)
    {
        return Task.FromResult(BuildPublicUrl(fileKey));
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private string BuildPublicUrl(string key)
    {
        if (!string.IsNullOrEmpty(_settings.PublicUrlBase))
        {
            return $"{_settings.PublicUrlBase.TrimEnd('/')}/{key}";
        }

        // Fall back: ServiceUrl/BucketName/key
        return $"{_settings.ServiceUrl.TrimEnd('/')}/{_settings.BucketName}/{key}";
    }

    private string ExtractKeyFromUrl(string url)
    {
        var prefix = !string.IsNullOrEmpty(_settings.PublicUrlBase)
            ? $"{_settings.PublicUrlBase.TrimEnd('/')}/"
            : $"{_settings.ServiceUrl.TrimEnd('/')}/{_settings.BucketName}/";

        return url.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? url[prefix.Length..]
            : url;
    }

    private static string SanitizeFileName(string fileName)
    {
        return Path.GetFileName(fileName).Replace(" ", "_");
    }
}
