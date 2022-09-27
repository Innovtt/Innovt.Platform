// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda;

public abstract class EventProcessor<T, TResult> : BaseEventProcessor where T : class
{
    protected EventProcessor(ILogger logger) : base(logger)
    {
    }

    protected EventProcessor()
    {
    }

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

    protected abstract Task<TResult> Handle(T message, ILambdaContext context);
}

public abstract class EventProcessor<T> : BaseEventProcessor where T : class
{
    protected EventProcessor(ILogger logger) : base(logger)
    {
    }

    protected EventProcessor()
    {
    }


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

    protected abstract Task Handle(T message, ILambdaContext context);
}