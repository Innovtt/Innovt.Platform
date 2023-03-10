// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Innovt.Cloud.AWS.Lambda;

public abstract class BaseEventProcessor
{
    protected static readonly ActivitySource EventProcessorActivitySource =
        new("Innovt.Cloud.AWS.Lambda.EventProcessor");

    private bool isIocContainerInitialized;

    protected BaseEventProcessor(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected BaseEventProcessor()
    {
    }

    protected ILogger Logger { get; private set; }
    protected ILambdaContext Context { get; set; }
    protected IConfigurationRoot Configuration { get; set; }

    protected void InitializeLogger(ILogger logger = null)
    {
        if (Logger is { } && logger is null)
            return;

        Logger = logger is null ? new LambdaLogger(Context.Logger) : logger;
    }

    protected void SetupIoc()
    {
        if (isIocContainerInitialized)
            return;

        Logger.Info("Initializing IOC Container.");

        var container = SetupIocContainer();

        if (container != null)
        {
            container.CheckConfiguration();

            InitializeLogger(container.Resolve<ILogger>());

            Logger.Info("IOC Container Initialized.");
        }
        else
        {
            Logger.Warning("IOC Container not found.");
        }

        isIocContainerInitialized = true;
    }

    protected Activity StartBaseActivity(string activityName)
    {
        if (activityName is null) throw new ArgumentNullException(nameof(activityName));

        var activity = EventProcessorActivitySource.StartActivity(activityName) ?? new Activity(activityName);
        activity?.SetTag("Lambda.FunctionName", Context.FunctionName);
        activity?.SetTag("Lambda.FunctionVersion", Context.FunctionVersion);
        activity?.SetTag("Lambda.LogStreamName", Context.LogStreamName);
        activity?.AddBaggage("Lambda.RequestId", Context.AwsRequestId);

        //setting request id as parentId.
        if (activity?.ParentId is null && Context.AwsRequestId != null)
        {
            activity?.SetParentId(Context.AwsRequestId);
            activity?.SetIdFormat(ActivityIdFormat.W3C);
        }

        activity?.Start();

        return activity;
    }

    protected virtual void SetupConfiguration()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddEnvironmentVariables();
        configBuilder.AddJsonFile("appsettings.json", true);

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (!string.IsNullOrWhiteSpace(environmentName))
            configBuilder.AddJsonFile(
                $"appsettings.{environmentName.ToLower(CultureInfo.CurrentCulture)}.json", true);

        EnrichConfiguration(configBuilder);

        Configuration = configBuilder.Build();
    }

    protected abstract IContainer SetupIocContainer();

    protected virtual void EnrichConfiguration(ConfigurationBuilder configurationBuilder)
    {
    }
}