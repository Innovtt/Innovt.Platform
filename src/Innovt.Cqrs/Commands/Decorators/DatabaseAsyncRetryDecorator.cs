using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Validation;
using Innovt.Cqrs.Decorators;
using Polly;

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class DatabaseAsyncRetryDecorator<TCommand> : BaseDatabaseRetryDecorator,
        IAsyncCommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IAsyncCommandHandler<TCommand> asyncCommandHandler;


        public DatabaseAsyncRetryDecorator(IAsyncCommandHandler<TCommand> commandHandler, ILogger logger,
            int retryCount = 3) : base(logger, retryCount)
        {
            this.asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
        {
            await CreateAsyncPolicy()
                .ExecuteAsync(async () => await asyncCommandHandler.Handle(command, cancellationToken));
        }
    }
}