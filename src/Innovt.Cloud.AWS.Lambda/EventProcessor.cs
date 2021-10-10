// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Microsoft.Extensions.Configuration;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class EventProcessor<T> where T : class
    {
        private bool isIocContainerInitialized;

        protected static readonly ActivitySource EventProcessorActivitySource = new("Innovt.Cloud.AWS.Lambda.EventProcessor");

        protected EventProcessor(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected EventProcessor()
        {
        }

        protected ILogger Logger { get; private set; }
        protected ILambdaContext Context { get; private set; }
        protected IConfigurationRoot Configuration { get; set; }

        private void InitializeLogger(ILogger logger =null) {

            if (Logger is { } && logger is null)
                return;
            
            Logger = logger is null ? new LambdaLogger(Context.Logger) : logger;                        
        }
        private void SetupIoc()
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
      
        public async Task Process(T message, ILambdaContext context)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context));

            Context = context;

            InitializeLogger();

            Logger.Info($"Receiving message. Function {context.FunctionName} and Version {context.FunctionVersion}");

            try
            {
                using var activity = EventProcessorActivitySource.StartActivity(nameof(Process));
                activity?.SetTag("Lambda.FunctionName", context.FunctionName);
                activity?.SetTag("Lambda.FunctionVersion", context.FunctionVersion);
                activity?.SetTag("Lambda.LogStreamName", context.LogStreamName);
                activity?.AddBaggage("Lambda.RequestId", context.AwsRequestId);                

                //setting request id as parentId.
                if (activity?.ParentId is null && context.AwsRequestId != null)
                {
                    activity?.SetParentId(context.AwsRequestId);
                }

                SetupIoc();

                await Handle(message, context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);                
                throw;
            }
        }

        protected abstract IContainer SetupIocContainer();

        protected abstract Task Handle(T message, ILambdaContext context);
    }
}