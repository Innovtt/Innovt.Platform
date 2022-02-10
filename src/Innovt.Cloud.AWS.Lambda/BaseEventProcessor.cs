// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class BaseEventProcessor
    {
        private bool isIocContainerInitialized;

        protected static readonly ActivitySource EventProcessorActivitySource = new("Innovt.Cloud.AWS.Lambda.EventProcessor");

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

            if (activityName is null)
            {
                throw new ArgumentNullException(nameof(activityName));
            }

            using var activity = EventProcessorActivitySource.StartActivity(activityName);
            activity?.SetTag("Lambda.FunctionName", Context.FunctionName);
            activity?.SetTag("Lambda.FunctionVersion", Context.FunctionVersion);
            activity?.SetTag("Lambda.LogStreamName", Context.LogStreamName);
            activity?.AddBaggage("Lambda.RequestId", Context.AwsRequestId);

            //setting request id as parentId.
            if (activity?.ParentId is null && Context.AwsRequestId != null)
            {
                activity?.SetParentId(Context.AwsRequestId);
            }

            return activity;
        }

        protected virtual void SetupConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddEnvironmentVariables();
            configBuilder.AddJsonFile($"appsettings.json", optional: true);

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                configBuilder.AddJsonFile($"appsettings.{environmentName.ToLower(System.Globalization.CultureInfo.CurrentCulture)}.json", optional: true);
            }

            EnrichConfiguration(configBuilder);

            Configuration = configBuilder.Build();
        }


        protected abstract IContainer SetupIocContainer();

        protected virtual void EnrichConfiguration(ConfigurationBuilder configurationBuilder) { }
    }
}