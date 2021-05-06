// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Kinesis
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

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