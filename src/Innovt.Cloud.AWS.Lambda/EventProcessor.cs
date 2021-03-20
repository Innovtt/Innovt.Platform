using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Microsoft.Extensions.Configuration;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class EventProcessor<T> where T : class
    {
        protected ILogger Logger { get; private set; }
        protected ILambdaContext Context { get; private set; }
        protected IConfigurationRoot Configuration { get; private set; }

        protected bool IsIocContainerInitialized = false;

        protected EventProcessor(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected EventProcessor()
        {
        }

        private void SetupIoc()
        {
            if (IsIocContainerInitialized)
            {
                Context.Logger.LogLine("IOC Container already initialized.");
                return;
            }

            Context.Logger.LogLine("Initializing IOC Container.");

            var container = SetupIocContainer();

            if (container != null)
            {
                container.CheckConfiguration();

                Logger = container.Resolve<ILogger>();

                Context.Logger.LogLine("IOC Container Initialized.");
            }
            else
            {
                Context.Logger.LogLine("IOC Container not found.");
            }

            if (Logger == null)
            {
                Context.Logger.LogLine(
                    "Custom Logger not initialized. Please provide the default logger using your IOC container.");
                Logger = new LambdaLogger(Context.Logger);
            }

            //Will check always
            IsIocContainerInitialized = true;
        }

        public async Task Process(T message, ILambdaContext context)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context));

            context.Logger.LogLine(
                $"Receiving message. Function {context.FunctionName} and Version {context.FunctionVersion}");

            this.Context = context;

            SetupIoc();

            await Handle(message, context);
        }


        protected abstract IContainer SetupIocContainer();

        protected abstract Task Handle(T message, ILambdaContext context);
    }
}