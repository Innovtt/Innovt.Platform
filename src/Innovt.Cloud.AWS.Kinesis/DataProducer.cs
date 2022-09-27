// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Kinesis

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

namespace Innovt.Cloud.AWS.Kinesis;

public class DataProducer<T> : AwsBaseService where T : class, IDataStream
{
    protected static readonly ActivitySource ActivityDataProducer = new("Innovt.Cloud.AWS.KinesisDataProducer");
    private AmazonKinesisClient kinesisClient;

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

    private static List<PutRecordsRequestEntry> CreatePutRecords(IList<T> dataStreams, Activity activity)
    {
        if (dataStreams == null)
            return null;

        var request = new List<PutRecordsRequestEntry>();

        foreach (var data in dataStreams)
        {
            if (data.TraceId.IsNullOrEmpty() && activity != null)
            {
                data.TraceId = activity.TraceId.ToString();
            }

            data.PublishedAt = DateTimeOffset.UtcNow;

            var dataAsBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<object>(data));

            using (var ms = new MemoryStream(dataAsBytes))
            {
                request.Add(new PutRecordsRequestEntry
                {
                    Data = ms,
                    PartitionKey = data.Partition
                });
            }
        }

        return request;
    }

    private async Task InternalPublish(IEnumerable<T> dataList, CancellationToken cancellationToken = default)
    {
        Logger.Info("Kinesis Publisher Started");

        if (dataList is null)
        {
            Logger.Info("The event list is empty or null.");
            return;
        }

        var dataStreams = dataList.ToList();

        if (dataStreams.Count > 500) throw new InvalidEventLimitException();

        using var activity = ActivityDataProducer.StartActivity(nameof(InternalPublish));
        activity?.SetTag("BusName", BusName);

        var request = new PutRecordsRequest
        {
            StreamName = BusName,
            Records = CreatePutRecords(dataStreams, activity)
        };

        Logger.Info($"Publishing Data for Bus {BusName}");

        var policy = base.CreateDefaultRetryAsyncPolicy();

        var results = await policy.ExecuteAsync(async () =>
                await KinesisClient.PutRecordsAsync(request, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        if (results.FailedRecordCount == 0)
        {
            Logger.Info($"All data published to Bus {BusName}");
            return;
        }

        foreach (var data in dataStreams)
        {
            data.PublishedAt = null;
        }

        var errorRecords = results.Records.Where(r => r.ErrorCode != null);

        foreach (var error in errorRecords)
        {
            Logger.Error($"Error publishing message. Error: {error.ErrorCode}, ErrorMessage: {error.ErrorMessage}");
        }
    }

    public async Task Publish(T data, CancellationToken cancellationToken = default)
    {
        await InternalPublish(new List<T> { data }, cancellationToken).ConfigureAwait(false);
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