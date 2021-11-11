// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class EventProcessor<T, TResult>: BaseEventProcessor where T : class
    {
        public async Task<TResult> Process(T message, ILambdaContext context)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context));

            Logger.Info($"Receiving message. Function {context.FunctionName} and Version {context.FunctionVersion}");

            Context = context;

            InitializeLogger();
            
            try
            {
                using var activity = StartBaseActivity(nameof(Process));

                SetupConfiguration();

                SetupIoc();

                return await Handle(message, context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw;
            }
        }

        protected abstract Task<TResult> Handle(T message, ILambdaContext context);
    }

    public abstract class EventProcessor<T>: BaseEventProcessor where T : class
    {   
        protected EventProcessor(ILogger logger):base(logger)
        {            
        }

        protected EventProcessor():base()
        {
        }       
      
      
        public async Task Process(T message, ILambdaContext context)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context));

            Logger.Info($"Receiving message. Function {context.FunctionName} and Version {context.FunctionVersion}");

            Context = context;

            InitializeLogger();            

            try
            {
                using var activity = base.StartBaseActivity(nameof(Process));

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
}