// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.S3.Tests

using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.File;
using Innovt.CrossCutting.Log.Serilog;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.S3.Tests;

[TestFixture]
[Ignore("Integrated Tests Only")]
public class Tests
{
    [Test]
    public async Task Upload()
    {
        const string bucketName = "cloud2gether-cdn-dev";

        var logger = new Logger();

        var configuration = new DefaultAwsConfiguration("c2g-dev");

        IFileSystem fileSystem = new S3FileSystem(logger, configuration);

        var fileName = "Sample.txt".ToLower();

        var fileExists = await fileSystem.FileExistsAsync(bucketName, fileName);

        if (fileExists)
        {
            await fileSystem.DeleteObjectAsync(bucketName, fileName);
        }

        var content = "sample storage class";
        using var ms = new MemoryStream();
        var sw = new StreamWriter(ms);

        await sw.WriteAsync(content);
        await sw.FlushAsync();
        var path = await fileSystem.UploadAsync(bucketName, ms, fileName, serverSideEncryptionMethod: "AES256");

        Assert.That(path, Is.Not.Null);
    }

    [Test]
    public async Task UploadAsJson()
    {
        const string bucketName = "cloud2gether-cdn-dev";

        var logger = new Logger();

        var configuration = new DefaultAwsConfiguration("c2g-dev");

        IFileSystem fileSystem = new S3FileSystem(logger, configuration);

        var fileName = "sample.json".ToLower();

        var fileExists = await fileSystem.FileExistsAsync(bucketName, fileName);

        if (fileExists)
        {
            await fileSystem.DeleteObjectAsync(bucketName, fileName);
        }

        var content = new { Name = "Sample", Description = "This is a sample JSON content" };

        var path = await fileSystem.UploadAsJsonAsync(bucketName, content, fileName,
            serverSideEncryptionMethod: "AES256");

        Assert.That(path, Is.Not.Null);
    }
}