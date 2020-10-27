using System;
using System.Data.SqlClient;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Polly;

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> :  ICommandHandler<TCommand> where TCommand: ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;
        private readonly ILogger logger;
        private readonly int retryCount;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> commandHandler,ILogger logger, int retryCount =3)
        {
            this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.retryCount = retryCount;
        }

        private Action<Exception, TimeSpan, int, Context> LogResiliencyRetry()
        {
            return (exception, timeSpan, retryCount, context) =>
            {
                logger.Warning($"Retry {retryCount} implemented of {context.PolicyKey} at {context.OperationKey} due to {exception}");
            };
        }


        public void Handle(TCommand command)
        {  
            var policy =  Policy.Handle<SqlException>() 
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),LogResiliencyRetry());

            policy.Execute(()=> commandHandler.Handle(command));
        }
    }
}
