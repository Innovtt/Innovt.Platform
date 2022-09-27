// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.S3

using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.S3;

public abstract class S3EventProcessor : EventProcessor<S3Event>
{
    protected S3EventProcessor(ILogger logger) : base(logger)
    {
    }

    protected S3EventProcessor()
    {
    }

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

    protected abstract Task ProcessMessage(S3Event.S3EventNotificationRecord message);
}