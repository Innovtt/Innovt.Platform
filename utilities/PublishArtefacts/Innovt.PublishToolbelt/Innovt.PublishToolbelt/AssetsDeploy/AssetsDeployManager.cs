using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Innovt.PublishToolbelt.AssetsDeploy
{
    public class AssetsDeployManager
    {
        private string assetsLocalPath = string.Empty;
        private string bucketPath = string.Empty;
        private string awsAccessKey = string.Empty;
        private string awsSecretKey = string.Empty;
        private string awsRegion = string.Empty;
            
        public void Deploy(List<Argument> args)
        {
            assetsLocalPath = ArgumentHelper.Find("-AssetsLocalPath", args);
            bucketPath      = ArgumentHelper.Find("-BucketPath", args);
            awsAccessKey    = ArgumentHelper.Find("-AwsAccessKey", args);
            awsSecretKey    = ArgumentHelper.Find("-AwsSecretKey", args);
            awsRegion = ArgumentHelper.Find("-AwsRegion", args);

            if (string.IsNullOrEmpty(assetsLocalPath) || string.IsNullOrEmpty(bucketPath) ||
                string.IsNullOrEmpty(awsAccessKey) ||
                string.IsNullOrEmpty(awsSecretKey) ||
                string.IsNullOrEmpty(awsRegion))
            {
                throw new Exception("The parameres are not defined. -AssetsLocalPath,-BucketPath,-AwsAccessKey,-AwsSecretKey,-AwsRegion");
            }

            var s3Client = new AmazonS3Client(new BasicAWSCredentials(awsAccessKey, awsSecretKey),
                RegionEndpoint.GetBySystemName(awsRegion));

            TransferUtility directoryTransferUtility = new TransferUtility(s3Client);

            directoryTransferUtility.UploadDirectory(new TransferUtilityUploadDirectoryRequest()
            {
                BucketName = bucketPath,
                Directory = assetsLocalPath,
                UploadFilesConcurrently = true,
                CannedACL = S3CannedACL.PublicRead, SearchOption = SearchOption.AllDirectories
            });
        }
    }
}
