// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Kinesis

using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Kinesis;

/// <summary>
///     Represents a data producer for publishing data to an Amazon Kinesis stream.
/// </summary>
/// <typeparam name="T">The type of data streams to be published.</typeparam>
public class DataProducer<T> : AwsBaseService where T : class, IDataStream
{
    protected static readonly ActivitySource ActivityDataProducer = new("Innovt.Cloud.AWS.KinesisDataProducer");
    private AmazonKinesisClient kinesisClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataProducer{T}" /> class with the specified bus name,
    ///     logger, and AWS configuration.
    /// </summary>
    /// <param name="busName">The name of the Kinesis data stream (bus) to which data will be published.</param>
    /// <param name="logger">The logger for logging informational and error messages.</param>
    /// <param name="configuration">The AWS configuration used to create AWS service clients.</param>
    protected DataProducer(string busName, ILogger logger, IAwsConfiguration configuration) : base(logger,
        configuration)
    {
        BusName = busName ?? throw new ArgumentNullException(nameof(busName));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataProducer{T}" /> class with the specified bus name,
    ///     logger, AWS configuration, and AWS region.
    /// </summary>
    /// <param name="busName">The name of the Kinesis data stream (bus) to which data will be published.</param>
    /// <param name="logger">The logger for logging informational and error messages.</param>
    /// <param name="configuration">The AWS configuration used to create AWS service clients.</param>
    /// <param name="region">The AWS region in which the Kinesis data stream is located.</param>
    protected DataProducer(string busName, ILogger logger, IAwsConfiguration configuration,
        string region) : base(logger, configuration, region)
    {
        BusName = busName ?? throw new ArgumentNullException(nameof(busName));
    }

    /// <summary>
    ///     Gets the name of the Kinesis data stream (bus) to which data will be published.
    /// </summary>
    private string BusName { get; }

    /// <summary>
    ///     Gets the Amazon Kinesis client for interacting with Kinesis streams.
    /// </summary>
    private AmazonKinesisClient KinesisClient
    {
        get { return kinesisClient ??= CreateService<AmazonKinesisClient>(); }
    }

    /// <summary>
    ///     Creates a list of <see cref="PutRecordsRequestEntry" /> from a collection of data streams.
    /// </summary>
    /// <param name="dataStreams">The collection of data streams to be converted.</param>
    /// <param name="activity">The activity used for tracing purposes.</param>
    /// <returns>A list of <see cref="PutRecordsRequestEntry" /> representing the data streams.</returns>
    private static List<PutRecordsRequestEntry> CreatePutRecords(IList<T> dataStreams, Activity activity)
    {
        if (dataStreams == null || !dataStreams.Any())
            return null;

        var request = new List<PutRecordsRequestEntry>();

        foreach (var data in dataStreams)
        {
            if (data.TraceId.IsNullOrEmpty() && activity != null) data.TraceId = activity.TraceId.ToString();

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

    /// <summary>
    ///     Publishes a collection of data streams to the Kinesis data stream asynchronously.
    /// </summary>
    /// <param name="dataList">The collection of data streams to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    private async Task InternalPublish(IEnumerable<T> dataList, CancellationToken cancellationToken = default)
    {
        Logger.Info("Kinesis Publisher Started");

        if (dataList is null || !dataList.Any())
        {
            Logger.Info("The event list is empty or null.");
            return;
        }

        var dataStreams = dataList.ToList();

        if (dataStreams.Count > 500) throw new InvalidEventLimitException();

        using var activity = ActivityDataProducer.StartActivity();
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

        foreach (var data in dataStreams) data.PublishedAt = null;

        var errorRecords = results.Records.Where(r => r.ErrorCode != null);

        foreach (var error in errorRecords)
            Logger.Error($"Error publishing message. Error: {error.ErrorCode}, ErrorMessage: {error.ErrorMessage}");
    }

    /// <summary>
    ///     Publishes a single data stream to the Kinesis data stream asynchronously.
    /// </summary>
    /// <param name="data">The data stream to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Task representing the asynchronous publish operation.</returns>
    public async Task Publish(T data, CancellationToken cancellationToken = default)
    {
        await InternalPublish(new List<T> { data }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Publishes a collection of data streams to the Kinesis data stream asynchronously.
    /// </summary>
    /// <param name="events">The collection of data streams to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Task representing the asynchronous publish operation.</returns>
    public async Task Publish(IEnumerable<T> events, CancellationToken cancellationToken = default)
    {
        await InternalPublish(events, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Disposes the Amazon Kinesis client when the service is no longer needed.
    /// </summary>
    protected override void DisposeServices()
    {
        kinesisClient?.Dispose();
    }
}