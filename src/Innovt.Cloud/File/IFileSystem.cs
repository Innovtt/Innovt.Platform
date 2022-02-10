// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.File
{
    public interface IFileSystem
    {
        Task<bool> CopyObject(string sourceBucket, string sourceKey, string destinationBucket, string destinationKey,
            string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default);

        void Download(string bucketName, string fileName, string destination);

        Stream DownloadStream(string url);

        Stream DownloadStream(string bucketName, string fileName);

        Task<Stream> DownloadStreamAsync(string url, CancellationToken cancellationToken = default);

        Task<Stream> DownloadStreamAsync(string bucketName, string fileName,
            CancellationToken cancellationToken = default);

        (string bucket, string fileKey) ExtractBucketFromGetUrl(string bucketUrl);

        bool FolderExists(string bucketName, string key);

        Task<bool> FolderExistsAsync(string bucketName, string key, CancellationToken cancellationToken = default);

        Task<bool> FileExistsAsync(string bucketName, string key, CancellationToken cancellationToken = default);

#pragma warning disable CA1055 // URI-like return values should not be strings
        string GeneratePreSignedUrl(string bucketName, string key, DateTime expiration,
#pragma warning restore CA1055 // URI-like return values should not be strings
            IDictionary<string, object> additionalProperties);

#pragma warning disable CA1055 // URI-like return values should not be strings
        string GetPreSignedUrl(string bucketName, string key, DateTime expires);
#pragma warning restore CA1055 // URI-like return values should not be strings

        string GetObjectContent(string url, Encoding encoding);

        Task<string> GetObjectContentAsync(string url, Encoding encoding,
            CancellationToken cancellationToken = default);

        T GetObjectFromJson<T>(Uri filePath);

        string PutObject(string bucketName, Stream stream, string fileName, string contentType = null,
            string serverSideEncryptionMethod = null, string fileAcl = null);

        string PutObject(string bucketName, string filePath, string contentType = null,
            string serverSideEncryptionMethod = null, string fileAcl = null);

        Task<string> PutObjectAsync(string bucketName, Stream stream, string fileName, string contentType = null,
            string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default);

        Task<string> PutObjectAsync(string bucketName, string filePath, string contentType = null,
            string serverSideEncryptionMethod = null, string fileAcl = null, CancellationToken cancellationToken = default);

        string Upload(string bucketName, Stream stream, string fileName,
            IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null, string fileAcl = null);

        string Upload(string bucketName, string filePath,
            IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null, string fileAcl = null);

        Task<string> UploadAsync(string bucketName, Stream stream, string fileName,
            IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null, string fileAcl = null,
            CancellationToken cancellationToken = default);

        Task<string> UploadAsync(string bucketName, string filePath,
            IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null, string fileAcl = null,
            CancellationToken cancellationToken = default);

        Task<string> UploadAsJsonAsync<T>(string bucketName, T obj, string fileName,
          IList<KeyValuePair<string, string>> metadata = null, string serverSideEncryptionMethod = null, string fileAcl = null,
          CancellationToken cancellationToken = default);


        Task UploadDirectoryAsync(string bucketName, string directory, string serverSideEncryptionMethod = null, string fileAcl = null,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default);

        bool DeleteObject(string bucketName, string key);
    }
}