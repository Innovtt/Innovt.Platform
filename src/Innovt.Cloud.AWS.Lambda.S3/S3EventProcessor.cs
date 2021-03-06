﻿using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3.Util;
using Innovt.Core.CrossCutting.Log;


namespace Innovt.Cloud.AWS.Lambda.S3
{
    public abstract class S3EventProcessor : EventProcessor<S3Event>
    {
        protected S3EventProcessor(ILogger logger) : base(logger)
        {
        }

        protected S3EventProcessor() : base()
        {
        }

        protected override async Task Handle(S3Event s3Event, ILambdaContext context)
        {
            //TODO: Ter controle de indepotencia.

            Logger.Info($"Processing S3Event With {s3Event.Records?.Count} records.");

            if (s3Event?.Records == null) return;
            if (s3Event.Records.Count == 0) return;

            foreach (var record in s3Event.Records)
            {
                Logger.Info($"Processing S3Event Version {record.EventVersion}");

                await ProcessMessage(record);

                Logger.Info($"Event from S3 processed. Version {record.EventVersion}");
            }
        }

        protected abstract Task ProcessMessage(S3EventNotification.S3EventNotificationRecord message);
    }
}