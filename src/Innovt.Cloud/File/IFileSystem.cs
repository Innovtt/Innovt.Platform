// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.File;

/// <summary>
///    Interface for File System
/// </summary>
public interface IFileSystem
{
    /// <summary>
    ///    Copy a file from one bucket to another
    /// </summary>
    /// <param name="sourceBucket"></param>
    /// <param name="sourceKey"></param>
    /// <param name="destinationBucket"></param>
    /// <param name="destinationKey"></param>
    /// <param name="serverSideEncryptionMethod"></param>
    /// <param name="fileAcl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CopyObject(string sourceBucket, string sourceKey, string destinationBucket, string destinationKey,
        string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file from the specified bucket and saves it to the destination path.
    /// </summary>
    void Download(string bucketName, string fileName, string destination);

    /// <summary>
    /// Downloads a file from the provided URL and returns the stream.
    /// </summary>
    Stream DownloadStream(string url);

    /// <summary>
    /// Downloads a file from the specified bucket and file name synchronously and returns the stream.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="fileName">The name of the file to download.</param>
    /// <returns>The stream of the downloaded file.</returns>
    Stream DownloadStream(string bucketName, string fileName);

    /// <summary>
    /// Downloads a file from the provided URL asynchronously and returns the stream.
    /// </summary>
    /// <param name="url">The URL of the file to download.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The stream of the downloaded file.</returns>
    Task<Stream> DownloadStreamAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file from the specified bucket and file name asynchronously and returns the stream.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="fileName">The name of the file to download.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The stream of the downloaded file.</returns>
    Task<Stream> DownloadStreamAsync(string bucketName, string fileName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts the bucket name and file key from the provided URL.
    /// </summary>
    /// <param name="bucketUrl">The URL containing bucket information.</param>
    /// <returns>A tuple containing the bucket name and file key.</returns>
    (string bucket, string fileKey) ExtractBucketFromGetUrl(string bucketUrl);

    /// <summary>
    /// Checks if a folder exists in the specified bucket.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="key">The key of the folder.</param>
    /// <returns>True if the folder exists; otherwise, false.</returns>
    bool FolderExists(string bucketName, string key);

    /// <summary>
    /// Checks if a folder exists in the specified bucket asynchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="key">The key of the folder.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>True if the folder exists; otherwise, false.</returns>
    Task<bool> FolderExistsAsync(string bucketName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a file exists in the specified bucket asynchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="key">The key of the file.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    Task<bool> FileExistsAsync(string bucketName, string key, CancellationToken cancellationToken = default);

#pragma warning disable CA1055 // URI-like return values should not be strings
    /// <summary>
    /// Generates a pre-signed URL for accessing the object in the specified bucket.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="key">The key of the object.</param>
    /// <param name="expiration">The expiration date and time for the pre-signed URL.</param>
    /// <param name="additionalProperties">Additional properties for customizing the URL generation.</param>
    /// <returns>The pre-signed URL for accessing the object.</returns>
    string GeneratePreSignedUrl(string bucketName, string key, DateTime expiration,
#pragma warning restore CA1055 // URI-like return values should not be strings
        IDictionary<string, object> additionalProperties);

#pragma warning disable CA1055 // URI-like return values should not be strings
    /// <summary>
    /// Gets a pre-signed URL for accessing the object in the specified bucket.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="key">The key of the object.</param>
    /// <param name="expires">The expiration date and time for the pre-signed URL.</param>
    /// <returns>The pre-signed URL for accessing the object.</returns>
    string GetPreSignedUrl(string bucketName, string key, DateTime expires);
#pragma warning restore CA1055 // URI-like return values should not be strings
    /// <summary>
    /// Gets the content of the object from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the object.</param>
    /// <param name="encoding">The encoding to use for reading the content.</param>
    /// <returns>The content of the object as a string.</returns>
    string GetObjectContent(string url, Encoding encoding);

    /// <summary>
    /// Gets the content of the object from the specified URL asynchronously.
    /// </summary>
    /// <param name="url">The URL of the object.</param>
    /// <param name="encoding">The encoding to use for reading the content.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The content of the object as a string.</returns>
    Task<string> GetObjectContentAsync(string url, Encoding encoding,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deserializes a JSON object from the content of a file specified by a URI.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="filePath">The URI pointing to the JSON file.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
    T GetObjectFromJson<T>(Uri filePath);

    /// <summary>
    /// Uploads a file from a stream to the specified bucket.
    /// </summary>
    string PutObject(string bucketName, Stream stream, string fileName, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null);

    /// <summary>
    /// Uploads a file from a local file path to the specified bucket.
    /// </summary>
    string PutObject(string bucketName, string filePath, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null);

    /// <summary>
    /// Uploads an object from a stream to the specified bucket asynchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="stream">The stream containing the object to upload.</param>
    /// <param name="fileName">The name of the file to upload.</param>
    /// <param name="contentType">The content type of the object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method to use.</param>
    /// <param name="fileAcl">The access control for the object.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The unique identifier of the uploaded object.</returns>
    Task<string> PutObjectAsync(string bucketName, Stream stream, string fileName, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads an object from a local file to the specified bucket asynchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="filePath">The local file path of the object to upload.</param>
    /// <param name="contentType">The content type of the object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method to use.</param>
    /// <param name="fileAcl">The access control for the object.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The unique identifier of the uploaded object.</returns>
    Task<string> PutObjectAsync(string bucketName, string filePath, string contentType = null,
        string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads an object from a stream to the specified bucket synchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="stream">The stream containing the object to upload.</param>
    /// <param name="fileName">The name of the file to upload.</param>
    /// <param name="metadata">Metadata for the object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method to use.</param>
    /// <param name="fileAcl">The access control for the object.</param>
    /// <returns>The unique identifier of the uploaded object.</returns>
    string Upload(string bucketName, Stream stream, string fileName,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null);

    /// <summary>
    /// Uploads an object from a local file to the specified bucket synchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="filePath">The local file path of the object to upload.</param>
    /// <param name="metadata">Metadata for the object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method to use.</param>
    /// <param name="fileAcl">The access control for the object.</param>
    /// <returns>The unique identifier of the uploaded object.</returns>
    string Upload(string bucketName, string filePath,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null);

    /// <summary>
    /// Uploads an object from a stream to the specified bucket asynchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="stream">The stream containing the object to upload.</param>
    /// <param name="fileName">The name of the file to upload.</param>
    /// <param name="metadata">Metadata for the object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method to use.</param>
    /// <param name="fileAcl">The access control for the object.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The unique identifier of the uploaded object.</returns>
    Task<string> UploadAsync(string bucketName, Stream stream, string fileName,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads an object from a local file to the specified bucket asynchronously.
    /// </summary>
    /// <param name="bucketName">The name of the bucket.</param>
    /// <param name="filePath">The local file path of the object to upload.</param>
    /// <param name="metadata">Metadata for the object.</param>
    /// <param name="serverSideEncryptionMethod">The server-side encryption method to use.</param>
    /// <param name="fileAcl">The access control for the object.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>The unique identifier of the uploaded object.</returns>
    Task<string> UploadAsync(string bucketName, string filePath,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads an object as JSON to the specified bucket.
    /// </summary>
    Task<string> UploadAsJsonAsync<T>(string bucketName, T obj, string fileName,
        IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads all files in a directory to the specified bucket.
    /// </summary>
    Task UploadDirectoryAsync(string bucketName, string directory, string serverSideEncryptionMethod = null,
        string fileAcl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an object from the specified bucket.
    /// </summary>
    Task<bool> DeleteObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an object from the specified bucket.
    /// </summary>
    bool DeleteObject(string bucketName, string key);
}