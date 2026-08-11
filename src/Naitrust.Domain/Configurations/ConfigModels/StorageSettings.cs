namespace Naitrust.Domain.Configurations.ConfigModels;

public class StorageSettings
{
    /// <summary>Storage provider identifier (e.g. "s3")</summary>
    public string Provider { get; set; } = "s3";

    /// <summary>S3-compatible service endpoint URL (Railway / MinIO)</summary>
    public string ServiceUrl { get; set; } = "";

    public string BucketName { get; set; } = "";
    public string AccessKey { get; set; } = "";
    public string SecretKey { get; set; } = "";

    /// <summary>AWS region — use "us-east-1" for non-AWS providers</summary>
    public string Region { get; set; } = "us-east-1";

    /// <summary>Base URL used to construct public file URLs (CDN or bucket public URL)</summary>
    public string PublicUrlBase { get; set; } = "";
}
