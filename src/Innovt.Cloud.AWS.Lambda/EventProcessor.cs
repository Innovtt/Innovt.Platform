// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda;

/// <summary>
/// Represents a base class for event processors that handle specific event types and produce a result.
/// </summary>
/// <typeparam name="T">The type of the event to process.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the event processing.</typeparam>
public abstract class EventProcessor<T, TResult> : BaseEventProcessor where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessor{T, TResult}"/> class with a logger.
    /// </summary>
    /// <param name="logger">The logger to use for logging events and errors.</param>
    protected EventProcessor(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessor{T, TResult}"/> class without a logger.
    /// </summary>
    protected EventProcessor()
    {
    }

    /// <summary>
    /// Processes the specified event message and produces a result.
    /// </summary>
    /// <param name="message">The event message to process.</param>
    /// <param name="context">The Lambda context associated with the event processing.</param>
    /// <returns>The result of processing the event message.</returns>
    public async Task<TResult> Process(T message, ILambdaContext context)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        Context = context ?? throw new ArgumentNullException(nameof(context));

        InitializeLogger();

        using var activity = StartBaseActivity(nameof(Process));

        Logger.Info($"Receiving message. Function {context.FunctionName} and Version {context.FunctionVersion}");

        try
        {
            SetupConfiguration();

            SetupIoc();

            return await Handle(message, context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            Logger.Error(ex, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Handles the event message and produces a result.
    /// </summary>
    /// <param name="message">The event message to handle.</param>
    /// <param name="context">The Lambda context associated with the event processing.</param>
    /// <returns>The result of handling the event message.</returns>
    protected abstract Task<TResult> Handle(T message, ILambdaContext context);
}

/// <summary>
/// Represents a base class for event processors that handle specific event types without producing a result.
/// </summary>
/// <typeparam name="T">The type of the event to process.</typeparam>
public abstract class EventProcessor<T> : BaseEventProcessor where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessor{T}"/> class with a logger.
    /// </summary>
    /// <param name="logger">The logger to use for logging events and errors.</param>
    protected EventProcessor(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessor{T}"/> class without a logger.
    /// </summary>
    protected EventProcessor()
    {
    }

    /// <summary>
    /// Processes the specified event message.
    /// </summary>
    /// <param name="message">The event message to process.</param>
    /// <param name="context">The Lambda context associated with the event processing.</param>
    public async Task Process(T message, ILambdaContext context)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        Context = context ?? throw new ArgumentNullException(nameof(context));

        InitializeLogger();

        Logger.Info($"Receiving message. Function {context.FunctionName} and Version {context.FunctionVersion}");

        try
        {
            using var activity = StartBaseActivity(nameof(Process));

            SetupConfiguration();

            SetupIoc();

            await Handle(message, context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Handles the event message.
    /// </summary>
    /// <param name="message">The event message to handle.</param>
    /// <param name="context">The Lambda context associated with the event processing.</param>
    protected abstract Task Handle(T message, ILambdaContext context);
}