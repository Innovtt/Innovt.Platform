using Innovt.CrossCutting.Log.Serilog;
using NUnit.Framework;
using System.IO;

namespace Innovt.Cloud.AWS.S3.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UploadWithStandardStorage()
        {
            const string bucketName = "datalake-raw-antecipa-v2";

            var logger = new Logger();
            var configuration = new Innovt.Cloud.AWS.Configuration.DefaultAWSConfiguration("antecipa-data");

            var fileSystem = new S3FileSystem(logger, configuration);

            var fileName = $"antecipa/v1/samplemichel.json".ToLower();

  
            var content = "sample storage class".ToString();

            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);
            sw.Write(content);
            sw.Flush();
            fileSystem.Upload(bucketName, ms, fileName, serverSideEncryptionMethod: "AES256");           
        }
    }
}