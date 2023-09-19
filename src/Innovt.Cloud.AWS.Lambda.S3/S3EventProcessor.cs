// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.S3

using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.S3;

/// <summary>
/// The abstract S3EventProcessor class serves as the base class for processing Amazon S3 events in AWS Lambda functions.
/// </summary>
public abstract class S3EventProcessor : EventProcessor<S3Event>
{
    /// <summary>
    /// Initializes a new instance of the S3EventProcessor class with the specified logger.
    /// </summary>
    /// <param name="logger">The logger used for logging events and messages.</param>
    protected S3EventProcessor(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the S3EventProcessor class with default settings.
    /// </summary>
    protected S3EventProcessor()
    {
    }

    /// <summary>
    /// Handles the processing of an incoming S3Event, which may contain multiple S3 event records.
    /// </summary>
    /// <param name="message">The S3Event containing one or more S3 event records.</param>
    /// <param name="context">The ILambdaContext providing information about the Lambda function's execution environment.</param>
    /// <returns>A Task representing the asynchronous processing operation.</returns>
    protected override async Task Handle(S3Event message, ILambdaContext context)
    {
        Logger.Info($"Processing S3Event With {message?.Records?.Count} records.");

        if (message?.Records == null) return;
        if (message.Records.Count == 0) return;

        using var activity = EventProcessorActivitySource.StartActivity(nameof(Handle));
        activity?.SetTag("S3MessageRecordsCount", message?.Records?.Count);

        foreach (var record in message.Records!)
        {
            Logger.Info($"Processing S3Event Version {record.EventVersion}");

            await ProcessMessage(record).ConfigureAwait(false);

            Logger.Info($"Event from S3 processed. Version {record.EventVersion}");
        }
    }

    /// <summary>
    /// Handles the processing of an individual S3 event record.
    /// </summary>
    /// <param name="message">The S3Event.S3EventNotificationRecord representing a single S3 event record.</param>
    /// <returns>A Task representing the asynchronous processing operation.</returns>
    protected abstract Task ProcessMessage(S3Event.S3EventNotificationRecord message);
}