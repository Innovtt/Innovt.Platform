using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Validation;
using Polly;

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class DatabaseAsyncRetryDecorator<TCommand> : IAsyncCommandHandler<TCommand> where TCommand: ICommand
    {
        private readonly IAsyncCommandHandler<TCommand> asyncCommandHandler;
        private readonly ILogger logger;
        private readonly int retryCount;

        public DatabaseAsyncRetryDecorator(IAsyncCommandHandler<TCommand> commandHandler,ILogger logger, int retryCount = 3)
        {
            this.asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
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


        public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.EnsureIsValid();

           var policy =   Policy.Handle<SqlException>().WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),LogResiliencyRetry());

           await policy.ExecuteAsync(async ()=> await asyncCommandHandler.Handle(command, cancellationToken));
        }
    }
}
