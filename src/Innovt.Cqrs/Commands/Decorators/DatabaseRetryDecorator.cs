using System;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> : BaseDatabaseRetryDecorator, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;


        public DatabaseRetryDecorator(ICommandHandler<TCommand> commandHandler, ILogger logger, int retryCount = 3) :
            base(logger, retryCount)
        {
            this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        public void Handle(TCommand command)
        {
            CreatePolicy().Execute(() => commandHandler.Handle(command));
        }
    }
}