// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.S3
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.File;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.S3;

public class S3FileSystem : AwsBaseService, IFileSystem
{
    private static readonly ActivitySource S3ActivitySource = new("Innovt.Cloud.AWS.S3.S3FileSystem");

    private AmazonS3Client s3Client;

    public S3FileSystem(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    public S3FileSystem(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }

    private AmazonS3Client S3Client
    {
        get { return s3Client ??= CreateService<AmazonS3Client>(); }
    }

    //only if in the same region
    public (string bucket, string fileKey) ExtractBucketFromGetUrl(string bucketUrl)
    {
        if (bucketUrl == null) throw new ArgumentNullException(nameof(bucketUrl));

        var amazonPrefix = ".amazonaws.com/";

        var urlWithoutPrefix = bucketUrl.Remove(0,
            bucketUrl.IndexOf(amazonPrefix, StringComparison.CurrentCultureIgnoreCase) + amazonPrefix.Length);

        var spitedUrl = urlWithoutPrefix.Split('/');

        var bucket = spitedUrl[0];

        var fileKey = urlWithoutPrefix.Replace(bucket + "/", "", StringComparison.OrdinalIgnoreCase);

        return (bucket, fileKey);
    }

    public string PutObject(string bucketName, string filePath, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        return AsyncHelper.RunSync(async () =>
            await PutObjectAsync(bucketName, filePath, contentType, serverSideEncryptionMethod, fileAcl)
                .ConfigureAwait(false));
    }

    public string PutObject(string bucketName, Stream stream, string fileName, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        return AsyncHelper.RunSync(async () =>
            await PutObjectAsync(bucketName, stream, fileName, contentType, serverSideEncryptionMethod, fileAcl)
                .ConfigureAwait(false));
    }

    public Task<string> PutObjectAsync(string bucketName, Stream stream, string fileName, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default)
    {
        if (bucketName == null) throw new ArgumentNullException(nameof(bucketName));
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        return PutObjectInternalAsync(bucketName, stream, fileName, contentType, serverSideEncryptionMethod, fileAcl,
            cancellationToken);
    }

    public Task<string> PutObjectAsync(string bucketName, string filePath, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        return PutObjectInternalAsync(bucketName, filePath, contentType, serverSideEncryptionMethod, fileAcl,
            cancellationToken);
    }

    public void Download(string bucketName, string fileName, string destination)
    {
        var request = new TransferUtilityDownloadRequest
        {
            BucketName = bucketName,
            FilePath = destination,
            Key = fileName
        };

        base.CreateDefaultRetryPolicy().Execute(() => new TransferUtility(S3Client).Download(request));
    }


    public Stream DownloadStream(string bucketName, string fileName)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.filename", fileName);

        var request = new TransferUtilityOpenStreamRequest
        {
            BucketName = bucketName,
            Key = fileName
        };

        return base.CreateDefaultRetryPolicy().Execute(() => new TransferUtility(S3Client).OpenStream(request));
    }

    public Stream DownloadStream(string url)
    {
        var (bucket, fileKey) = ExtractBucketFromGetUrl(url);

        return DownloadStream(bucket, fileKey);
    }

    public async Task<Stream> DownloadStreamAsync(string bucketName, string fileName,
        CancellationToken cancellationToken = default)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.filename", fileName);

        var request = new TransferUtilityOpenStreamRequest
        {
            BucketName = bucketName,
            Key = fileName
        };

        return await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(
                async () => await new TransferUtility(S3Client).OpenStreamAsync(request, cancellationToken)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    public Task<Stream> DownloadStreamAsync(string url, CancellationToken cancellationToken = default)
    {
        var (bucket, fileKey) = ExtractBucketFromGetUrl(url);

        return DownloadStreamAsync(bucket, fileKey, cancellationToken);
    }

    public async Task<string> GetObjectContentAsync(string url, Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        var stream = await DownloadStreamAsync(url, cancellationToken).ConfigureAwait(false);

        using var reader = new StreamReader(stream, encoding);

        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    public string GetObjectContent(string url, Encoding encoding)
    {
        var stream = DownloadStream(url);

        using var reader = new StreamReader(stream, encoding);

        return reader.ReadToEnd();
    }

    /// <summary>
    ///     When you need to get a content from Json file as an typed method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public T GetObjectFromJson<T>(Uri filePath)
    {
        if (filePath is null) throw new ArgumentNullException(nameof(filePath));

        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.url", filePath);

        var content = GetObjectContent(filePath.ToString(), Encoding.UTF8);

        if (content is null)
            return default;

        return JsonSerializer.Deserialize<T>(content);
    }

    public string GetPreSignedUrl(string bucketName, string key, DateTime expires)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.key", key);
        activity?.SetTag("s3.expire", expires.ToString("O"));

        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = expires
        };

        return base.CreateDefaultRetryPolicy().Execute(() => S3Client.GetPreSignedURL(request));
    }

    public string GeneratePreSignedUrl(string bucketName, string key, DateTime expiration,
        IDictionary<string, object> additionalProperties)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.key", key);
        activity?.SetTag("s3.expire", expiration.ToString("O"));

        return base.CreateDefaultRetryPolicy().Execute(() =>
            ((IAmazonS3)S3Client).GeneratePreSignedURL(bucketName, key, expiration, additionalProperties));
    }

    public async Task UploadDirectoryAsync(string bucketName, string directory,
        string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.directory", directory);

        var request = new TransferUtilityUploadDirectoryRequest
        {
            BucketName = bucketName,
            Directory = directory,
            UploadFilesConcurrently = true,
            CannedACL = fileAcl is null ? null : new S3CannedACL(fileAcl),
            ServerSideEncryptionMethod = serverSideEncryptionMethod is null
                ? null
                : new ServerSideEncryptionMethod(serverSideEncryptionMethod)
        };

        var policy = base.CreateDefaultRetryAsyncPolicy();

        await policy.ExecuteAsync(async () =>
                await new TransferUtility(S3Client).UploadDirectoryAsync(request, cancellationToken)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    public string Upload(string bucketName, Stream stream, string fileName,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.filename", fileName);

        var request = CreateUploadRequest(bucketName, stream, fileName, metadata, serverSideEncryptionMethod, fileAcl);

        base.CreateDefaultRetryPolicy().Execute(() => new TransferUtility(S3Client).Upload(request));

        return GetObjectUrl(bucketName, fileName);
    }

    public string Upload(string bucketName, string filePath, IList<KeyValuePair<string, string>> metadata = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        var fileName = Path.GetFileName(filePath);

        using var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return Upload(bucketName, fileToUpload, fileName, metadata, serverSideEncryptionMethod, fileAcl);
    }

    public async Task<string> UploadAsync(string bucketName, Stream stream, string fileName,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateUploadRequest(bucketName, stream, fileName, metadata, serverSideEncryptionMethod, fileAcl);

        await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await new TransferUtility(S3Client).UploadAsync(request, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        return GetObjectUrl(bucketName, fileName);
    }

    public Task<string> UploadAsync(string bucketName, string filePath,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        var fileName = Path.GetFileName(filePath);

        using var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return UploadAsync(bucketName, fileToUpload, fileName, metadata, serverSideEncryptionMethod, fileAcl,
            cancellationToken);
    }

    public async Task<bool> FolderExistsAsync(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        var response = await ListObjectsAsync(bucketName, key, cancellationToken).ConfigureAwait(false);

        return response.MaxKeys > 0;
    }

    public async Task<bool> FileExistsAsync(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        var response = await ListObjectsAsync(bucketName, key, cancellationToken).ConfigureAwait(false);

        return response.S3Objects?.Count > 0;
    }

    public bool FolderExists(string bucketName, string key)
    {
        return AsyncHelper.RunSync(async () => await FolderExistsAsync(bucketName, key).ConfigureAwait(false));
    }

    public async Task<bool> DeleteObjectAsync(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.key", key);

        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        var response = await base.CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(
                async () => await S3Client.DeleteObjectAsync(request, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public bool DeleteObject(string bucketName, string key)
    {
        return AsyncHelper.RunSync(async () => await DeleteObjectAsync(bucketName, key).ConfigureAwait(false));
    }

    public async Task<bool> CopyObject(string sourceBucket, string sourceKey, string destinationBucket,
        string destinationKey, string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        using var activity = S3ActivitySource.StartActivity("DeleteObjectAsync");
        activity?.SetTag("s3.source_bucket", sourceBucket);
        activity?.SetTag("s3.source_key", sourceKey);
        activity?.SetTag("s3.destination_bucket", destinationBucket);
        activity?.SetTag("s3.destination_key", destinationKey);

        var request = new CopyObjectRequest
        {
            SourceBucket = sourceBucket,
            SourceKey = sourceKey,
            DestinationBucket = destinationBucket,
            DestinationKey = destinationKey,
            CannedACL = fileAcl is null ? null : new S3CannedACL(fileAcl),
            ServerSideEncryptionMethod = serverSideEncryptionMethod is null
                ? null
                : new ServerSideEncryptionMethod(serverSideEncryptionMethod)
        };

        var response =
            await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () =>
                    await S3Client.CopyObjectAsync(request, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<string> UploadAsJsonAsync<T>(string bucketName,
        T obj,
        string fileName,
        IList<KeyValuePair<string, string>> metadata = null,
        string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(JsonSerializer.Serialize(obj)).ConfigureAwait(false);
        await writer.FlushAsync().ConfigureAwait(false);

        return await UploadAsync(bucketName, stream, fileName, metadata, serverSideEncryptionMethod, fileAcl,
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<ListObjectsV2Response> ListObjectsAsync(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        using var activity = S3ActivitySource.StartActivity();
        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.key", key);

        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            MaxKeys = 1,
            Prefix = key
        };

        return
            await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await S3Client.ListObjectsV2Async(request, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
    }

    private string GetObjectUrl(string bucketName, string fileKey)
    {
        return "https://s3.amazonaws.com/" + $"{bucketName}/{fileKey}";
    }

    private async Task<string> PutObjectInternalAsync(string bucketName, [NotNull] string filePath,
        string contentType = null, string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        var fileName = Path.GetFileName(filePath);

        return await PutObjectAsync(bucketName, stream, fileName, contentType, serverSideEncryptionMethod, fileAcl,
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> PutObjectInternalAsync(string bucketName, Stream stream, string fileName,
        string contentType = null, string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        using var activity = S3ActivitySource.StartActivity();

        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.filename", fileName);

        var fileKey = Path.GetFileName(fileName);

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey,
            InputStream = stream,
            ContentType = contentType,
            AutoCloseStream = true,
            CannedACL = fileAcl is null ? null : new S3CannedACL(fileAcl),
            ServerSideEncryptionMethod = serverSideEncryptionMethod is null
                ? null
                : new ServerSideEncryptionMethod(serverSideEncryptionMethod)
        };

        var result =
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await S3Client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);

        if (result.HttpStatusCode != HttpStatusCode.OK)
            throw new FileNotFoundException();

        return GetObjectUrl(bucketName, fileKey);
    }


    private static TransferUtilityUploadRequest CreateUploadRequest(string bucketName, Stream stream,
        string fileName, IList<KeyValuePair<string, string>> metadata = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        using var activity = S3ActivitySource.StartActivity();

        activity?.SetTag("s3.bucket_name", bucketName);
        activity?.SetTag("s3.filename", fileName);

        var request = new TransferUtilityUploadRequest
        {
            BucketName = bucketName,
            StorageClass = S3StorageClass.Standard,
            InputStream = stream,
            AutoResetStreamPosition = true,
            AutoCloseStream = true,
            Key = fileName,
            CannedACL = fileAcl is null ? null : new S3CannedACL(fileAcl),
            ServerSideEncryptionMethod = serverSideEncryptionMethod == null
                ? null
                : new ServerSideEncryptionMethod(serverSideEncryptionMethod)
        };

        if (metadata == null) return request;

        foreach (var (key, value) in metadata) request.Metadata.Add(key, value);

        return request;
    }


    protected override void DisposeServices()
    {
        s3Client?.Dispose();
    }
}