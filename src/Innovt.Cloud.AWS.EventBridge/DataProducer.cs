// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.EventBridge

using System.Diagnostics;
using System.Text.Json;
using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.EventBridge;

/// <summary>
///     Represents a data producer for publishing data to an Amazon EventBridge event bus.
/// </summary>
/// <typeparam name="T">The type of data streams to be published.</typeparam>
public class DataProducer<T> : AwsBaseService where T : class, IDataStream
{
    private readonly ActivitySource activityDataProducer = new("Innovt.Cloud.AWS.EventBridgeDataProducer");
    private AmazonEventBridgeClient eventBridgeClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataProducer{T}" /> class with the specified bus name,
    ///     logger, and AWS configuration.
    /// </summary>
    /// <param name="busName">The name of the EventBridge event bus to which data will be published.</param>
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
    /// <param name="busName">The name of the EventBridge event bus to which data will be published.</param>
    /// <param name="logger">The logger for logging informational and error messages.</param>
    /// <param name="configuration">The AWS configuration used to create AWS service clients.</param>
    /// <param name="region">The AWS region in which the EventBridge event bus is located.</param>
    protected DataProducer(string busName, ILogger logger, IAwsConfiguration configuration,
        string region) : base(logger, configuration, region)
    {
        BusName = busName ?? throw new ArgumentNullException(nameof(busName));
    }

    /// <summary>
    ///     Gets the name of the EventBridge event bus to which data will be published.
    /// </summary>
    private string BusName { get; }

    /// <summary>
    ///     Gets the Amazon EventBridge client for interacting with EventBridge.
    /// </summary>
    private AmazonEventBridgeClient EventBridgeClient
    {
        get { return eventBridgeClient ??= CreateService<AmazonEventBridgeClient>(); }
    }

    /// <summary>
    ///     Creates a list of <see cref="PutEventsRequestEntry" /> from a collection of data streams.
    /// </summary>
    /// <param name="dataStreams">The collection of data streams to be converted.</param>
    /// <param name="activity">The activity used for tracing purposes.</param>
    /// <param name="busName">The EventBridge event bus name.</param>
    /// <returns>A list of <see cref="PutEventsRequestEntry" /> representing the data streams.</returns>
    private static List<PutEventsRequestEntry>? CreatePutEventsEntries(IList<T> dataStreams, Activity? activity,
        string busName)
    {
        if (dataStreams.IsNullOrEmpty())
            return null;

        var entries = new List<PutEventsRequestEntry>();

        foreach (var data in dataStreams.Where(d=>d!=null!))
        {
            if (data.TraceId.IsNullOrEmpty() && activity != null) 
                data.TraceId = activity.TraceId.ToString();

            data.PublishedAt = DateTimeOffset.UtcNow;

            var detail = JsonSerializer.Serialize<object>(data);

            entries.Add(new PutEventsRequestEntry
            {
                Source = data.Partition,
                DetailType = data.GetType().Name,
                Detail = detail,
                EventBusName = busName,
                Time = DateTime.UtcNow,
                TraceHeader = data.TraceId
            });
        }

        return entries;
    }

    /// <summary>
    ///     Publishes a collection of data streams to the EventBridge event bus asynchronously.
    /// </summary>
    /// <param name="dataList">The collection of data streams to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    private async Task InternalPublish(IEnumerable<T> dataList, CancellationToken cancellationToken = default)
    {
        Logger.Info("EventBridge Publisher Started");

        var dataStreams = dataList as T[] ?? dataList.ToArray();

        if (dataStreams.IsNullOrEmpty())
        {
            Logger.Info("The event list is empty or null.");
            return;
        }

        if (dataStreams.Length > 10) throw new InvalidEventLimitException();

        using var activity = activityDataProducer.StartActivity();
        activity?.SetTag("BusName", BusName);

        var request = new PutEventsRequest
        {
            Entries = CreatePutEventsEntries(dataStreams, activity, BusName)
        };

        Logger.Info($"Publishing Data for Bus {BusName}");

        var policy = base.CreateDefaultRetryAsyncPolicy();

        var results = await policy.ExecuteAsync(async () =>
                await EventBridgeClient.PutEventsAsync(request, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
        
        if (results.FailedEntryCount == 0)
        {
            Logger.Info($"All data published to Bus {BusName}");
            return;
        }

        foreach (var data in dataStreams) data.PublishedAt = null;

        var errorEntries = results.Entries.Where(e => e.ErrorCode != null);

        foreach (var error in errorEntries)
            Logger.Error($"Error publishing event to EventBridge. Error: {error.ErrorCode}, ErrorMessage: {error.ErrorMessage}");
    }

    /// <summary>
    ///     Publishes a single data stream to the EventBridge event bus asynchronously.
    /// </summary>
    /// <param name="data">The data stream to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Task representing the asynchronous publish operation.</returns>
    public async Task Publish(T data, CancellationToken cancellationToken = default)
    {
        await InternalPublish(new List<T> { data }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Publishes a collection of data streams to the EventBridge event bus asynchronously.
    /// </summary>
    /// <param name="events">The collection of data streams to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Task representing the asynchronous publish operation.</returns>
    public async Task Publish(IEnumerable<T> events, CancellationToken cancellationToken = default)
    {
        await InternalPublish(events, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Disposes the Amazon EventBridge client when the service is no longer needed.
    /// </summary>
    protected override void DisposeServices()
    {
        eventBridgeClient?.Dispose();
        activityDataProducer?.Dispose();
    }
}
