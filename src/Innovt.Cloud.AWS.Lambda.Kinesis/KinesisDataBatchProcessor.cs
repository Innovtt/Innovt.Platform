// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Streams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisDataBatchProcessor<TBody> : KinesisDataProcessorBase<TBody> where TBody : class
    {
        protected KinesisDataBatchProcessor(ILogger logger) : base(logger)
        {
        }
        protected KinesisDataBatchProcessor()
        {
        }

        private async Task<List<DataStream<TBody>>> CreateBatchMessages(IEnumerable<KinesisEvent.KinesisEventRecord> messageRecords)
        {
            using (EventProcessorActivitySource.StartActivity(nameof(ParseRecord)))
            {
                var dataStreams = new List<DataStream<TBody>>();

                foreach (var record in messageRecords)
                {
                    dataStreams.Add(await ParseRecord<DataStream<TBody>>(record).ConfigureAwait(false));
                }

                return dataStreams;
            }
        }

        protected override async Task Handle(KinesisEvent message, ILambdaContext context)
        {
            Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");

            if (message?.Records == null) return;
            if (message.Records.Count == 0) return;

            Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");
            var batchMessages = await CreateBatchMessages(message.Records).ConfigureAwait(false);
            Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");

            using var activity = EventProcessorActivitySource.StartActivity(nameof(ProcessMessage));
            activity?.SetTag("BatchMessagesCount", batchMessages.Count);

            await ProcessMessage(batchMessages).ConfigureAwait(false);
        }

        protected abstract Task ProcessMessage(IList<DataStream<TBody>> message);
    }
}