// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Kinesis

using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Kinesis;

/// <summary>
/// The event handler for processing domain events and publishing them to an AWS Kinesis stream.
/// </summary>
public class EventHandler : DataProducer<DomainEvent>, IEventHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandler"/> class with the specified parameters.
    /// </summary>
    /// <param name="busName">The name of the AWS Kinesis stream to publish events to.</param>
    /// <param name="logger">The logger instance for logging events.</param>
    /// <param name="configuration">The AWS configuration for accessing AWS services.</param>
    public EventHandler(string busName, ILogger logger, IAwsConfiguration configuration) : base(busName, logger,
        configuration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandler"/> class with the specified parameters.
    /// </summary>
    /// <param name="busName">The name of the AWS Kinesis stream to publish events to.</param>
    /// <param name="logger">The logger instance for logging events.</param>
    /// <param name="configuration">The AWS configuration for accessing AWS services.</param>
    /// <param name="region">The AWS region to use.</param>
    public EventHandler(string busName, ILogger logger, IAwsConfiguration configuration,
        string region) : base(busName, logger, configuration, region)
    {
    }
}