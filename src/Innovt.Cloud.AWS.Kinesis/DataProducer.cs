// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Kinesis
{
    public class DataProducer<T> : AwsBaseService where T : class, IDataStream
    {
        private AmazonKinesisClient kinesisClient;

        public static readonly ActivitySource ActivityDataProducer = new("Innovt.Cloud.AWS.KinesisDataProducer");

        protected DataProducer(string busName, ILogger logger, IAwsConfiguration configuration) : base(logger,
            configuration)
        {
            BusName = busName ?? throw new ArgumentNullException(nameof(busName));
        }

        protected DataProducer(string busName, ILogger logger, IAwsConfiguration configuration,
            string region) : base(logger, configuration, region)
        {
            BusName = busName ?? throw new ArgumentNullException(nameof(busName));
        }

        private string BusName { get; }

        private AmazonKinesisClient KinesisClient
        {
            get { return kinesisClient ??= CreateService<AmazonKinesisClient>(); }
        }


        private async Task InternalPublish(IEnumerable<T> dataList, CancellationToken cancellationToken = default)
        {
            if (dataList == null) throw new ArgumentNullException(nameof(dataList));

            var dataStreams = dataList.ToList();

            if (dataStreams.Count > 500) throw new InvalidEventLimitException();

            Logger.Info("Kinesis Publisher Started");

            using var activity = ActivityDataProducer.StartActivity(nameof(InternalPublish));
            activity?.SetTag("BusName", BusName);

            var request = new PutRecordsRequest
            {
                StreamName = BusName,
                Records = new List<PutRecordsRequestEntry>()
            };

            foreach (var data in dataStreams)
            {
                activity?.SetTag("BusName", BusName);
                    
                if (activity?.ParentId is null && data.TraceId.IsNotNullOrEmpty())
                {
                    activity?.SetParentId(data.TraceId);
                }

                var dataAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<object>(data));
                await using var ms = new MemoryStream(dataAsBytes);
                request.Records.Add(new PutRecordsRequestEntry
                {
                    Data = ms,
                    PartitionKey = data.Partition
                });
            }

            Logger.Info($"Publishing Data for Bus {BusName}");

            var policy = base.CreateDefaultRetryAsyncPolicy();

            var results = await policy.ExecuteAsync(async () =>
                await KinesisClient.PutRecordsAsync(request, cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

            if (results.FailedRecordCount == 0)
            {
                Logger.Info($"All data published to Bus {BusName}");
                return;
            }

            var errorRecords = results.Records.Where(r => r.ErrorCode != null);

            foreach (var error in errorRecords)
            {
                Logger.Error($"Error publishing message. Error: {error.ErrorCode}, ErrorMessage: {error.ErrorMessage}");
            }
        }

        public async Task Publish(T data, CancellationToken cancellationToken = default)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Logger.Info("Sending Domain Event", data);

            await InternalPublish(new List<T> {data}, cancellationToken).ConfigureAwait(false);
        }

        public async Task Publish(IEnumerable<T> events, CancellationToken cancellationToken = default)
        {
            await InternalPublish(events, cancellationToken).ConfigureAwait(false);

        }

        protected override void DisposeServices()
        {
            kinesisClient?.Dispose();
        }
    }
}