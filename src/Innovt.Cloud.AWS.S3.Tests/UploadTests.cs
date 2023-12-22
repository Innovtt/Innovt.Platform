// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.S3.Tests

using NUnit.Framework;

namespace Innovt.Cloud.AWS.S3.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    //[Test]
    //public async Task UploadWithStandardStorage()
    //{
    //    const string bucketName = "datalake-raw-antecipa-v2";

    //    var logger = new Logger();
    //    var configuration = new DefaultAWSConfiguration("antecipa-data");

    //    IFileSystem fileSystem = new S3FileSystem(logger, configuration);

    //    //var fileName = $"antecipa/v1/samplemichel.json".ToLower();
    //    var fileName = "antecipa/v1/user/107532f24b15ca16ab4a7057d0be7ca9s.json".ToLower();

    //    //107532f24b15ca16ab4a7057d0be7ca9.json

    //    var exist = await fileSystem.FileExistsAsync(bucketName, fileName);

    //    var content = "sample storage class";

    //    using var ms = new MemoryStream();
    //    using var sw = new StreamWriter(ms);
    //    sw.Write(content);
    //    sw.Flush();
    //    fileSystem.Upload(bucketName, ms, fileName, serverSideEncryptionMethod: "AES256");
    //}


    //[Test]
    //public async Task UploadAsJson()
    //{
    //    const string bucketName = "processed-tests";

    //    var logger = new Logger();
    //    var configuration = new DefaultAWSConfiguration("antecipa-dev");

    //    //var fileName = $"antecipa/v1/samplemichel.json".ToLower();
    //    var fileName = "userMichelDto.json".ToLower();

    //    //107532f24b15ca16ab4a7057d0be7ca9.json
    //    var dto = new UserDto { Name = "michel", LastName = "Borges" };

    //    IFileSystem fileSystem = new S3FileSystem(logger, configuration);

    //    var url = await fileSystem.UploadAsJsonAsync(bucketName, dto, fileName);

    //    var expected = fileSystem.GetObjectFromJson<UserDto>(new Uri(url));
    //}
}