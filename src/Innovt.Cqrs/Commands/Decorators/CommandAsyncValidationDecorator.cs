// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

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
            asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.EnsureIsValid();

            await asyncCommandHandler.Handle(command, cancellationToken).ConfigureAwait(false);
        }
    }
}