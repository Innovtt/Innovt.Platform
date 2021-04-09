using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Validation;
using System;

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class CommandValidationDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;

        public CommandValidationDecorator(ICommandHandler<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        public void Handle(TCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.EnsureIsValid();

            commandHandler.Handle(command);
        }
    }
}