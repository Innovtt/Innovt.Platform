// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System;
using System.Diagnostics;
using System.Globalization;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Microsoft.Extensions.Configuration;

namespace Innovt.Cloud.AWS.Lambda;

/// <summary>
/// Represents a base class for event processors with common functionality such as logging, IOC container setup, and configuration.
/// </summary>
public abstract class BaseEventProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEventProcessor"/> class with an optional logger.
    /// </summary>
    protected static readonly ActivitySource EventProcessorActivitySource =
        new("Innovt.Cloud.AWS.Lambda.EventProcessor");

    private bool isIocContainerInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEventProcessor"/> class.
    /// </summary>
    protected BaseEventProcessor(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Allows to use the configuration and logger from outside. In This case, the method SetupConfiguration will not be called.
    /// </summary>
    /// <param name="logger">The logger provider.</param>
    /// <param name="configuration">The configuration to be used.</param>
    /// <exception cref="ArgumentNullException">If the logger or configuration is null.</exception>
    protected BaseEventProcessor(ILogger logger, IConfigurationRoot configuration) : this(logger)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Allows to use the configuration from outside. In This case, the method SetupConfiguration will not be called.
    /// </summary>
    /// <param name="configuration">The configuration to be used.</param>
    /// <exception cref="ArgumentNullException">If the logger or configuration is null.</exception>
    protected BaseEventProcessor(IConfigurationRoot configuration)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    protected BaseEventProcessor()
    {
    }

    /// <summary>
    /// Gets or sets the logger for recording processing information.
    /// </summary>
    protected ILogger Logger { get; private set; }

    /// <summary>
    /// Gets or sets the Lambda context associated with the event processing.
    /// </summary>
    protected ILambdaContext Context { get; set; }

    /// <summary>
    /// Gets or sets the configuration root for accessing application settings.
    /// </summary>
    protected IConfigurationRoot Configuration { get; set; }

    /// <summary>
    /// Initializes the logger with an optional logger instance or creates a new logger if not provided.
    /// </summary>
    /// <param name="logger">An optional logger instance to use.</param>
    protected void InitializeLogger(ILogger logger = null)
    {
        if (Logger is not null && logger is null)
            return;

        Logger = logger ?? new LambdaLogger(Context.Logger);
    }

    /// <summary>
    /// Sets up the Inversion of Control (IOC) container for managing dependencies.
    /// </summary>
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

    /// <summary>
    /// Starts a new activity for tracing and adds relevant tags and baggage information.
    /// </summary>
    /// <param name="activityName">The name of the activity to start.</param>
    /// <returns>The started <see cref="Activity"/>.</returns>
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

    /// <summary>
    /// Sets up the application configuration using environment variables and JSON files. When the configuration is already set, this method does nothing.
    /// </summary>
    protected virtual void SetupConfiguration()
    {
        //In this case the configuration is already set.
        if (Configuration is not null)
            return;

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddEnvironmentVariables();
        configBuilder.AddJsonFile("appsettings.json", true);

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (!string.IsNullOrWhiteSpace(environmentName))
            configBuilder.AddJsonFile($"appsettings.{environmentName.ToLower(CultureInfo.CurrentCulture)}.json", true);

        EnrichConfiguration(configBuilder);

        Configuration = configBuilder.Build();
    }

    /// <summary>
    /// Sets up the Inversion of Control (IOC) container for managing dependencies.
    /// </summary>
    /// <returns>The configured IOC container.</returns>
    protected abstract IContainer SetupIocContainer();

    /// <summary>
    /// Configures additional sources for enriching the application configuration.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder to which additional sources can be added.</param>
    protected virtual void EnrichConfiguration(ConfigurationBuilder configurationBuilder)
    {
    }
}