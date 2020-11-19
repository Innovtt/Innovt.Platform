using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Streams;
using OpenTracing;

namespace Innovt.Cloud.AWS.Kinesis
{
    public class DataProducer<T>:AwsBaseService  where T : class, IDataStream
    {
        private string BusName { get; set; }

        public DataProducer(string busName,ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {  
            this.BusName = busName ?? throw new ArgumentNullException(nameof(busName));
        }

        public DataProducer(string busName, ILogger logger,ITracer tracer, IAWSConfiguration configuration, string region) : base(logger,tracer,
            configuration, region)
        {
            this.BusName = busName ?? throw new ArgumentNullException(nameof(busName));
        }

        private AmazonKinesisClient kinesisClient;
        private AmazonKinesisClient KinesisClient
        {
            get { return kinesisClient ??= CreateService<AmazonKinesisClient>(); }
        }

        private async Task InternalPublish(IList<T> dataList, CancellationToken cancellationToken = default)
        {
            if (dataList == null) throw new ArgumentNullException(nameof(dataList));
            if (dataList.Count > 500) throw new InvalidEventLimitException();

            Logger.Info("Kinesis Publisher Started");

            var request = new Amazon.Kinesis.Model.PutRecordsRequest()
            {
                StreamName = BusName,
                Records =  new List<PutRecordsRequestEntry>()
            };

            foreach (var data in dataList)
            {
                if (data.TraceId.IsNullOrEmpty())
                    data.TraceId = base.GetTraceId();
                
                var dataAsBytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(data));
                await using var ms = new MemoryStream(dataAsBytes);

                request.Records.Add(new PutRecordsRequestEntry()
                {
                   Data = ms, 
                   PartitionKey = data.Partition
                });
            }

            Logger.Info("Publishing Data for Bus @BusName", BusName);

            var policy = base.CreateDefaultRetryAsyncPolicy();

            var results = await policy.ExecuteAsync(async () => await KinesisClient.PutRecordsAsync(request, cancellationToken));

            if (results.FailedRecordCount == 0)
            {
                Logger.Info("All data published to Bus @BusName", BusName);
                return;
            }

            var errorRecords = results.Records.Where(r => r.ErrorCode != null);

            foreach (var error in errorRecords)
            {
                Logger.Error("Error publishing message. Error: @ErrorCode, ErrorMessage: @ErrorMessage ",error.ErrorCode,error.ErrorMessage);
            }
        }

        public async Task Publish(T data,CancellationToken cancellationToken = default)
        {
            if (@data == null) throw new ArgumentNullException(nameof(@data));

            Logger.Info("Sending Domain Event @name", @data);

            await InternalPublish(new List<T>(){ @data }, cancellationToken);
        }

        public async Task Publish(IList<T> events, CancellationToken cancellationToken = default)
        {
            await InternalPublish(events, cancellationToken);
        }

        protected override void DisposeServices()
        {
            kinesisClient?.Dispose();
        }
    }
}