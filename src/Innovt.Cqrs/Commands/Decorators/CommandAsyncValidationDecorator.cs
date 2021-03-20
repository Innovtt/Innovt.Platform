using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class CommandAsyncValidationDecorator<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly IAsyncCommandHandler<TCommand> asyncCommandHandler;

        public CommandAsyncValidationDecorator(IAsyncCommandHandler<TCommand> commandHandler)
        {
            this.asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.EnsureIsValid();

            await asyncCommandHandler.Handle(command, cancellationToken).ConfigureAwait(false);
        }
    }
}