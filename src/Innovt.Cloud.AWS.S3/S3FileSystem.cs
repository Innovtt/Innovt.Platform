using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Innovt.Cloud.File;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.S3
{
    public class S3FileSystem : AwsBaseService, IFileSystem
    {
        public S3FileSystem(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
        }

        public S3FileSystem(ILogger logger, IAWSConfiguration configuration, string region) : base(logger,
            configuration, region)
        {
        }

        private AmazonS3Client _s3Client;

        private AmazonS3Client S3Client
        {
            get { return _s3Client ??= CreateService<AmazonS3Client>(); }
        }

        private string GetObjectUrl(string bucketName, string fileKey)
        {
            return "https://s3.amazonaws.com/" + $"{bucketName}/{fileKey}";
        }

        //only if in the same region
        public (string bucket, string fileKey) ExtractBucketFromGetUrl(string url)
        {
            var amazonPrefix = ".amazonaws.com/";

            var urlWithoutPrefix = url.Remove(0,
                url.IndexOf(amazonPrefix, StringComparison.CurrentCultureIgnoreCase) + amazonPrefix.Length);

            var spitedUrl = urlWithoutPrefix.Split('/');

            var bucket = spitedUrl[0];

            var fileKey = urlWithoutPrefix.Replace(bucket + "/", "");

            return (bucket, fileKey);
        }

        public string PutObject(string bucketName, string filePath, string contentType = null)
        {
            return AsyncHelper.RunSync<string>(async () => await PutObjectAsync(bucketName, filePath, contentType));
        }

        public string PutObject(string bucketName, Stream stream, string fileName, string contentType = null)
        {
            return AsyncHelper.RunSync<string>(async () =>
                await PutObjectAsync(bucketName, stream, fileName, contentType));
        }


        internal async Task<string> PutObjectInternalAsync(string bucketName, Stream stream, string fileName,
            string contentType = null, CancellationToken cancellationToken = default)
        {
            var fileKey = Path.GetFileName(fileName);

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                CannedACL = S3CannedACL.BucketOwnerFullControl,
                InputStream = stream,
                ContentType = contentType,
                AutoCloseStream = true
            };

            var policy = base.CreateDefaultRetryAsyncPolicy();

            var result =
                await policy.ExecuteAsync(async () => await S3Client.PutObjectAsync(request, cancellationToken));

            if (result.HttpStatusCode != HttpStatusCode.OK)
                throw new FileNotFoundException();

            return GetObjectUrl(bucketName, fileKey);
        }

        internal async Task<string> PutObjectInternalAsync(string bucketName, [NotNull] string filePath,
            string contentType = null, CancellationToken cancellationToken = default)
        {
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var fileName = Path.GetFileName(filePath);

            return await PutObjectAsync(bucketName, stream, fileName, contentType, cancellationToken);
        }

        public Task<string> PutObjectAsync(string bucketName, Stream stream, string fileName, string contentType = null,
            CancellationToken cancellationToken = default)
        {
            if (bucketName == null) throw new ArgumentNullException(nameof(bucketName));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return PutObjectInternalAsync(bucketName, stream, fileName, contentType, cancellationToken);
        }

        public Task<string> PutObjectAsync(string bucketName, string filePath, string contentType = null,
            CancellationToken cancellationToken = default)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            return PutObjectInternalAsync(bucketName, filePath, contentType, cancellationToken);
        }

        /// <summary>
        /// Download for local folder
        /// </summary>
        /// <param name="bucketName">The Bucket </param>
        /// <param name="fileName">The file key</param>
        /// <param name="destination">The File path </param>
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
            var request = new TransferUtilityOpenStreamRequest()
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
            var request = new TransferUtilityOpenStreamRequest()
            {
                BucketName = bucketName,
                Key = fileName
            };

            var policy = base.CreateDefaultRetryAsyncPolicy();

            return await policy.ExecuteAsync(async () =>
                await new TransferUtility(S3Client).OpenStreamAsync(request, cancellationToken));
        }

        public async Task<Stream> DownloadStreamAsync(string url, CancellationToken cancellationToken = default)
        {
            var (bucket, fileKey) = ExtractBucketFromGetUrl(url);

            return await DownloadStreamAsync(bucket, fileKey, cancellationToken);
        }

        public async Task<string> GetObjectContentAsync(string url, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            var stream = await DownloadStreamAsync(url, cancellationToken);

            using var reader = new StreamReader(stream, encoding);

            return await reader.ReadToEndAsync();
        }

        public string GetObjectContent(string url, Encoding encoding)
        {
            var stream = DownloadStream(url);

            using var reader = new StreamReader(stream, encoding);

            return reader.ReadToEnd();
        }

        private TransferUtilityUploadRequest CreateUploadRequest(string bucketName, Stream stream, string fileName,
            List<KeyValuePair<string, string>> metadata = null)
        {
            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                StorageClass = S3StorageClass.StandardInfrequentAccess,
                InputStream = stream,
                AutoResetStreamPosition = true,
                AutoCloseStream = true,
                Key = fileName
            };

            if (metadata == null) return request;

            foreach (var (key, value) in metadata) request.Metadata.Add(key, value);

            return request;
        }

        public string GetPreSignedURL(string bucketName, string key, DateTime expires)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = key,
                Expires = expires
            };

            return base.CreateDefaultRetryPolicy().Execute(() => S3Client.GetPreSignedURL(request));
        }

        public string GeneratePreSignedURL(string bucketName, string key, DateTime expiration,
            IDictionary<string, object> additionalProperties)
        {
            return base.CreateDefaultRetryPolicy().Execute(() =>
                ((IAmazonS3) S3Client).GeneratePreSignedURL(bucketName, key, expiration, additionalProperties));
        }

        public async Task UploadDirectoryAsync(string bucketName, string directory,
            CancellationToken cancellationToken = default)
        {
            var request = new TransferUtilityUploadDirectoryRequest()
            {
                BucketName = bucketName,
                Directory = directory,
                UploadFilesConcurrently = true
            };

            var policy = base.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () =>
                await new TransferUtility(S3Client).UploadDirectoryAsync(request, cancellationToken));
        }

        public string Upload(string bucketName, Stream stream, string fileName, string region = null,
            List<KeyValuePair<string, string>> metadata = null)
        {
            var request = CreateUploadRequest(bucketName, stream, fileName, metadata);

            base.CreateDefaultRetryPolicy().Execute(() => new TransferUtility(S3Client).Upload(request));

            return GetObjectUrl(bucketName, fileName);
        }

        public string Upload(string bucketName, string filePath, string region = null,
            List<KeyValuePair<string, string>> metadata = null)
        {
            var fileName = Path.GetFileName(filePath);

            using var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return Upload(bucketName, fileToUpload, fileName, region, metadata);
        }

        public async Task<string> UploadAsync(string bucketName, Stream stream, string fileName, string region = null,
            List<KeyValuePair<string, string>> metadata = null, CancellationToken cancellationToken = default)
        {
            var request = CreateUploadRequest(bucketName, stream, fileName, metadata);

            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await new TransferUtility(S3Client).UploadAsync(request, cancellationToken));

            return GetObjectUrl(bucketName, fileName);
        }

        public async Task<string> UploadAsync(string bucketName, string filePath, string region = null,
            List<KeyValuePair<string, string>> metadata = null, CancellationToken cancellationToken = default)
        {
            var fileName = Path.GetFileName(filePath);

            await using var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return await UploadAsync(bucketName, fileToUpload, fileName, region, metadata, cancellationToken);
        }

        public async Task<bool> FolderExistsAsync(string bucketName, string key, string region = null,
            List<KeyValuePair<string, string>> metadata = null, CancellationToken cancellationToken = default)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                MaxKeys = 1,
                Prefix = key
            };

            var policy = base.CreateDefaultRetryAsyncPolicy();

            var response =
                await policy.ExecuteAsync(async () => await S3Client.ListObjectsV2Async(request, cancellationToken));

            return response.MaxKeys > 0;
        }

        public bool FolderExists(string bucketName, string key)
        {
            return AsyncHelper.RunSync<bool>(async () => await FolderExistsAsync(bucketName, key));
        }

        public async Task<bool> DeleteObjectAsync(string bucketName, string key,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            var response = await base.CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () => await S3Client.DeleteObjectAsync(request, cancellationToken));

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public bool DeleteObject(string bucketName, string key)
        {
            return AsyncHelper.RunSync<bool>(async () => await DeleteObjectAsync(bucketName, key));
        }

        public async Task<bool> CopyObject(string sourceBucket, string sourceKey, string destinationBucket,
            string destinationKey, CancellationToken cancellationToken = default)
        {
            var request = new CopyObjectRequest()
            {
                SourceBucket = sourceBucket,
                SourceKey = sourceKey,
                DestinationBucket = destinationBucket,
                DestinationKey = destinationKey
            };

            var policy = base.CreateDefaultRetryAsyncPolicy();

            var response =
                await policy.ExecuteAsync(async () => await S3Client.CopyObjectAsync(request, cancellationToken));

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        protected override void DisposeServices()
        {
            _s3Client?.Dispose();
        }
    }
}