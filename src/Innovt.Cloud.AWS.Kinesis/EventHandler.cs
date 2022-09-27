// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Kinesis

using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Kinesis;

public class EventHandler : DataProducer<DomainEvent>, IEventHandler
{
    public EventHandler(string busName, ILogger logger, IAwsConfiguration configuration) : base(busName, logger,
        configuration)
    {
    }

    public EventHandler(string busName, ILogger logger, IAwsConfiguration configuration,
        string region) : base(busName, logger, configuration, region)
    {
    }
}