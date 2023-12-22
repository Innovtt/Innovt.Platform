// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.S3

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

/// <summary>
/// Amazon S3 file system implementation.
/// </summary>
public class S3FileSystem : AwsBaseService, IFileSystem
{
    private static readonly ActivitySource S3ActivitySource = new("Innovt.Cloud.AWS.S3.S3FileSystem");

    private AmazonS3Client s3Client;

    /// <summary>
    /// Initializes a new instance of the <see cref="S3FileSystem"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The AWS configuration.</param>
    public S3FileSystem(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="S3FileSystem"/> class with a specific AWS region.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The AWS configuration.</param>
    /// <param name="region">The AWS region.</param>
    public S3FileSystem(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }

    /// <summary>
    /// Gets the Amazon S3 client instance, creating a new instance if not already initialized.
    /// </summary>
    private AmazonS3Client S3Client
    {
        get { return s3Client ??= CreateService<AmazonS3Client>(); }
    }

    /// <summary>
    /// Extracts bucket name and file key from the S3 bucket URL.
    /// </summary>
    /// <param name="bucketUrl">The S3 bucket URL.</param>
    /// <returns>A tuple containing the bucket name and the file key.</returns>
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

    /// <summary>
    /// Uploads a file to an Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="filePath">The local file path of the file to upload.</param>
    /// <param name="contentType">The content type of the file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control list (optional).</param>
    /// <returns>The key of the uploaded object in the S3 bucket.</returns>
    public string PutObject(string bucketName, string filePath, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        return AsyncHelper.RunSync(async () =>
            await PutObjectAsync(bucketName, filePath, contentType, serverSideEncryptionMethod, fileAcl)
                .ConfigureAwait(false));
    }

    /// <summary>
    /// Uploads a stream to an Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="stream">The stream to upload.</param>
    /// <param name="fileName">The name to assign to the uploaded file.</param>
    /// <param name="contentType">The content type of the file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control list (optional).</param>
    /// <returns>The key of the uploaded object in the S3 bucket.</returns>
    public string PutObject(string bucketName, Stream stream, string fileName, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        return AsyncHelper.RunSync(async () =>
            await PutObjectAsync(bucketName, stream, fileName, contentType, serverSideEncryptionMethod, fileAcl)
                .ConfigureAwait(false));
    }

    /// <summary>
    /// Asynchronously uploads a stream to an Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="stream">The stream to upload.</param>
    /// <param name="fileName">The name to assign to the uploaded file.</param>
    /// <param name="contentType">The content type of the file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control list (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The key of the uploaded object in the S3 bucket.</returns>
    public Task<string> PutObjectAsync(string bucketName, Stream stream, string fileName, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default)
    {
        if (bucketName == null) throw new ArgumentNullException(nameof(bucketName));
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        return PutObjectInternalAsync(bucketName, stream, fileName, contentType, serverSideEncryptionMethod, fileAcl,
            cancellationToken);
    }

    /// <summary>
    /// Asynchronously uploads a file to an Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="filePath">The local file path of the file to upload.</param>
    /// <param name="contentType">The content type of the file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control list (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The key of the uploaded object in the S3 bucket.</returns>
    public Task<string> PutObjectAsync(string bucketName, string filePath, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        return PutObjectInternalAsync(bucketName, filePath, contentType, serverSideEncryptionMethod, fileAcl,
            cancellationToken);
    }

    /// <summary>
    /// Downloads a file from an Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="fileName">The name of the file to download.</param>
    /// <param name="destination">The local file path to save the downloaded file.</param>
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

    /// <summary>
    /// Downloads a file from an Amazon S3 bucket as a stream.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="fileName">The name of the file to download.</param>
    /// <returns>A stream containing the downloaded file's content.</returns>
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

    /// <summary>
    /// Downloads a file from an Amazon S3 bucket as a stream using the provided URL.
    /// </summary>
    /// <param name="url">The URL of the file to download from Amazon S3.</param>
    /// <returns>A stream containing the downloaded file's content.</returns>
    public Stream DownloadStream(string url)
    {
        var (bucket, fileKey) = ExtractBucketFromGetUrl(url);

        return DownloadStream(bucket, fileKey);
    }

    /// <summary>
    /// Asynchronously downloads a file from an Amazon S3 bucket as a stream.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="fileName">The name of the file to download.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>A stream containing the downloaded file's content.</returns>
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

    /// <summary>
    /// Asynchronously downloads a file from an Amazon S3 bucket as a stream using the provided URL.
    /// </summary>
    /// <param name="url">The URL of the file to download from Amazon S3.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>A stream containing the downloaded file's content.</returns>
    public Task<Stream> DownloadStreamAsync(string url, CancellationToken cancellationToken = default)
    {
        var (bucket, fileKey) = ExtractBucketFromGetUrl(url);

        return DownloadStreamAsync(bucket, fileKey, cancellationToken);
    }

    /// <summary>
    /// Asynchronously gets the content of an object from a specified URL with the given encoding.
    /// </summary>
    /// <param name="url">The URL of the object to retrieve.</param>
    /// <param name="encoding">The encoding to use for reading the object's content.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The content of the object as a string.</returns>
    public async Task<string> GetObjectContentAsync(string url, Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        var stream = await DownloadStreamAsync(url, cancellationToken).ConfigureAwait(false);

        using var reader = new StreamReader(stream, encoding);

        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the content of an object from a specified URL with the given encoding.
    /// </summary>
    /// <param name="url">The URL of the object to retrieve.</param>
    /// <param name="encoding">The encoding to use for reading the object's content.</param>
    /// <returns>The content of the object as a string.</returns>
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

    /// <summary>
    /// Gets a pre-signed URL for accessing an object in the S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key of the object in the S3 bucket.</param>
    /// <param name="expires">The expiration date and time for the pre-signed URL.</param>
    /// <returns>The pre-signed URL for accessing the object.</returns>
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

    /// <summary>
    /// Generates a pre-signed URL for accessing an object in the S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key of the object in the S3 bucket.</param>
    /// <param name="expiration">The expiration date and time for the pre-signed URL.</param>
    /// <param name="additionalProperties">Additional properties for the pre-signed URL (optional).</param>
    /// <returns>The pre-signed URL for accessing the object.</returns>
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

    /// <summary>
    /// Asynchronously uploads a directory to the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="directory">The path of the local directory to upload.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded files (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Uploads a file to the specified Amazon S3 bucket from a stream.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="stream">The stream containing the file content to upload.</param>
    /// <param name="fileName">The name of the file to upload.</param>
    /// <param name="metadata">Metadata key-value pairs for the uploaded file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded file (optional).</param>
    /// <returns>The URL of the uploaded file.</returns>
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

    /// <summary>
    /// Uploads a file to the specified Amazon S3 bucket from a local file path.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="filePath">The local file path to upload.</param>
    /// <param name="metadata">Metadata key-value pairs for the uploaded file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded file (optional).</param>
    /// <returns>The URL of the uploaded file.</returns>
    public string Upload(string bucketName, string filePath, IList<KeyValuePair<string, string>> metadata = null,
        string serverSideEncryptionMethod = null, string fileAcl = null)
    {
        var fileName = Path.GetFileName(filePath);

        using var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return Upload(bucketName, fileToUpload, fileName, metadata, serverSideEncryptionMethod, fileAcl);
    }

    /// <summary>
    /// Asynchronously uploads a file to the specified Amazon S3 bucket from a stream.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="stream">The stream containing the file content to upload.</param>
    /// <param name="fileName">The name of the file to upload.</param>
    /// <param name="metadata">Metadata key-value pairs for the uploaded file (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded file (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The URL of the uploaded file.</returns>
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

    /// <summary>
    /// Asynchronously uploads an object from a local file path to the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="filePath">The local file path to upload.</param>
    /// <param name="metadata">Metadata key-value pairs for the uploaded object (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded object (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The URL of the uploaded object.</returns>
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

    /// <summary>
    /// Asynchronously checks if a folder exists in the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key representing the folder in the S3 bucket.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>True if the folder exists; otherwise, false.</returns>
    public async Task<bool> FolderExistsAsync(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        var response = await ListObjectsAsync(bucketName, key, cancellationToken).ConfigureAwait(false);

        return response.MaxKeys > 0;
    }

    /// <summary>
    /// Asynchronously checks if a file exists in the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key representing the file in the S3 bucket.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    public async Task<bool> FileExistsAsync(string bucketName, string key,
        CancellationToken cancellationToken = default)
    {
        var response = await ListObjectsAsync(bucketName, key, cancellationToken).ConfigureAwait(false);

        return response.S3Objects?.Count > 0;
    }

    /// <summary>
    /// Synchronously checks if a folder exists in the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key representing the folder in the S3 bucket.</param>
    /// <returns>True if the folder exists; otherwise, false.</returns>
    public bool FolderExists(string bucketName, string key)
    {
        return AsyncHelper.RunSync(async () => await FolderExistsAsync(bucketName, key).ConfigureAwait(false));
    }

    /// <summary>
    /// Asynchronously deletes an object (file or folder) from the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key of the object to delete.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>True if the object was successfully deleted; otherwise, false.</returns>
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

    /// <summary>
    /// Synchronously deletes an object (file or folder) from the specified Amazon S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key of the object to delete.</param>
    /// <returns>True if the object was successfully deleted; otherwise, false.</returns>
    public bool DeleteObject(string bucketName, string key)
    {
        return AsyncHelper.RunSync(async () => await DeleteObjectAsync(bucketName, key).ConfigureAwait(false));
    }

    /// <summary>
    /// Asynchronously copies an object (file or folder) from the source bucket to the destination bucket.
    /// </summary>
    /// <param name="sourceBucket">The source bucket name.</param>
    /// <param name="sourceKey">The source key of the object to copy.</param>
    /// <param name="destinationBucket">The destination bucket name.</param>
    /// <param name="destinationKey">The destination key for the copied object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the copied object (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>True if the object was successfully copied; otherwise, false.</returns>
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

    /// <summary>
    /// Asynchronously uploads an object serialized as JSON to the specified Amazon S3 bucket.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="obj">The object to serialize and upload.</param>
    /// <param name="fileName">The name of the file to use for the uploaded object.</param>
    /// <param name="metadata">Metadata key-value pairs for the uploaded object (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded object (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The URL of the uploaded object.</returns>
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

    /// <summary>
    /// Asynchronously lists objects in the specified Amazon S3 bucket matching the provided key.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The prefix to use for filtering objects.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The response containing the list of objects.</returns>
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

    /// <summary>
    /// Gets the URL for accessing the object in the specified bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="fileKey">The key for the object in the bucket.</param>
    /// <returns>The URL to access the object.</returns>
    private string GetObjectUrl(string bucketName, string fileKey)
    {
        return "https://s3.amazonaws.com/" + $"{bucketName}/{fileKey}";
    }

    /// <summary>
    /// Asynchronously puts an object in the specified Amazon S3 bucket from a local file path.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="filePath">The local file path of the object to upload.</param>
    /// <param name="contentType">The content type of the object (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded object (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The URL of the uploaded object.</returns>
    private async Task<string> PutObjectInternalAsync(string bucketName, [NotNull] string filePath,
        string contentType = null, string serverSideEncryptionMethod = null, string fileAcl = null,
        CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        var fileName = Path.GetFileName(filePath);

        return await PutObjectAsync(bucketName, stream, fileName, contentType, serverSideEncryptionMethod, fileAcl,
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously puts an object in the specified Amazon S3 bucket from a stream.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="stream">The stream to read the object data from.</param>
    /// <param name="fileName">The name of the file to use for the uploaded object.</param>
    /// <param name="contentType">The content type of the object (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded object (optional).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>The URL of the uploaded object.</returns
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

    /// <summary>
    /// Creates a TransferUtilityUploadRequest for uploading an object to the specified Amazon S3 bucket from a stream.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="stream">The stream to read the object data from.</param>
    /// <param name="fileName">The name of the file to use for the uploaded object.</param>
    /// <param name="metadata">Metadata key-value pairs for the uploaded object (optional).</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method (optional).</param>
    /// <param name="fileAcl">The file access control for the uploaded object (optional).</param>
    /// <returns>The TransferUtilityUploadRequest for uploading the object.</returns>
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

    /// <summary>
    /// Disposes the S3 client instance.
    /// </summary>
    protected override void DisposeServices()
    {
        s3Client?.Dispose();
    }
}