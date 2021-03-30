using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Kinesis
{
    public class EventHandler : DataProducer<DomainEvent>, IEventHandler
    {
        public EventHandler(string busName, ILogger logger, IAWSConfiguration configuration) : base(busName, logger,
            configuration)
        {
        }

        public EventHandler(string busName, ILogger logger, IAWSConfiguration configuration,
            string region) : base(busName, logger, configuration, region)
        {
        }
    }
}