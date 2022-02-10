using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using System;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Sqs.Tests
{
    public class CustomSqsEventProcessor : SqsEventProcessor<Person>
    {
        public CustomSqsEventProcessor(ILogger logger) : base(logger)
        {
        }
        public CustomSqsEventProcessor(bool isFifo) : base(isFifo)
        {
        }
        public CustomSqsEventProcessor() : base()
        {
        }


        protected override IContainer SetupIocContainer()
        {
            return null;
        }

        protected override Task ProcessMessage(QueueMessage<Person> message)
        {
            //fake rule to test some conditions.

            if (message.Body.Name == "michel")
            {
                return Task.CompletedTask;
            }
            else
            {
                throw new Exception("Fake exception");
            }

        }
    }
}
