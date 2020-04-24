using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class EventProcessor<T> where T : class
    {
        protected ILogger Logger { get; private set; }
        protected ILambdaContext Context { get; private set; }

        protected bool IsIocContainerInitialized = false;

        protected EventProcessor(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        protected EventProcessor()
        {
        }

        protected void SetupIoc(ILambdaContext context)
        {
            context.Logger.LogLine("Initializing IOC Container.");

            if (IsIocContainerInitialized)
            {
                context.Logger.LogLine("IOC Container already initialized.");
                return;
            }

            Context = context;

            var container = SetupIocContainer();

            if (container != null)
            {
                container.CheckConfiguration();

                Logger = container.Resolve<ILogger>();

                if (Logger == null)
                {
                    context.Logger.LogLine("Custom Logger not initialized. Please provide the default logger using your IOC container.");
                    Logger = new LambdaLogger(context.Logger);

                }
            }

            IsIocContainerInitialized = true;

            context.Logger.LogLine("IOC Container Initialized.");
        }


        /// <summary>
        /// The Default Function Will be Namespace::Class::Process
        /// </summary>
        /// <param name="message">The message received</param>
        /// <param name="context">The lambda context</param>
        /// <returns></returns>
        public async Task Process(T message, ILambdaContext context)
        {   
            SetupIoc(context);

            await Handle(message, context);
        }

        protected abstract IContainer SetupIocContainer();
        public abstract Task Handle(T message, ILambdaContext context);
    }
}