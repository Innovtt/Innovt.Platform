// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.EventBridge

using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.EventBridge;

/// <summary>
///     The event handler for processing domain events and publishing them to an AWS EventBridge event bus.
/// </summary>
public class EventBridgeEventHandler : DataProducer<DomainEvent>, IEventHandler
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EventBridgeEventHandler" /> class with the specified parameters.
    /// </summary>
    /// <param name="busName">The name of the AWS EventBridge event bus to publish events to.</param>
    /// <param name="logger">The logger instance for logging events.</param>
    /// <param name="configuration">The AWS configuration for accessing AWS services.</param>
    public EventBridgeEventHandler(string busName, ILogger logger, IAwsConfiguration configuration) : base(busName, logger,
        configuration)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventBridgeEventHandler" /> class with the specified parameters.
    /// </summary>
    /// <param name="busName">The name of the AWS EventBridge event bus to publish events to.</param>
    /// <param name="logger">The logger instance for logging events.</param>
    /// <param name="configuration">The AWS configuration for accessing AWS services.</param>
    /// <param name="region">The AWS region to use.</param>
    public EventBridgeEventHandler(string busName, ILogger logger, IAwsConfiguration configuration,
        string region) : base(busName, logger, configuration, region)
    {
    }
}