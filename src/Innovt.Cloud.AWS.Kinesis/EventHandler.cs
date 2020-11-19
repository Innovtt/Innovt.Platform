using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;
using OpenTracing;

namespace Innovt.Cloud.AWS.Kinesis
{
    public class EventHandler:DataProducer<DomainEvent>, IEventHandler
    {
        public EventHandler(string busName, ILogger logger, IAWSConfiguration configuration) : base(busName, logger, configuration)
        {
           
        }

        public EventHandler(string busName, ILogger logger, ITracer tracer, IAWSConfiguration configuration, string region) : base(busName, logger, tracer, configuration, region)
        {
        }
        
    }
}